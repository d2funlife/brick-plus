#region Using statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Tetris.Helpers;
using Tetris.Models.Core;
using System.Text.RegularExpressions;
using Tetris.Enums;
#endregion

namespace Tetris.Managers
{
    public class RecordManager
    {
        public async Task<ObservableCollection<GameResult>> GetResults(Mode mode)
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(mode + ".txt");
                if (file == null) return new ObservableCollection<GameResult>();
                var properties = await file.GetBasicPropertiesAsync();
                if (properties.Size == 0) return new ObservableCollection<GameResult>();
                var fileContent = await FileIO.ReadTextAsync(file);
                var json = JsonSerializeHelper.Deserialize<List<GameResult>>(fileContent) ?? new List<GameResult>();
                return new ObservableCollection<GameResult>(json);
            }
            catch (FileNotFoundException)
            {
                return new ObservableCollection<GameResult>();
            }
        }

        public async Task<ObservableCollection<GameResult>> UpdateResultsAsync(ObservableCollection<GameResult> results, GameResult newResult, Mode gameMode)
        {
            newResult.PlayerName = await GetValidPlayerNameAsync(newResult.PlayerName);
            SettingsManager.SavePlayerName(newResult.PlayerName);

            var lastResult = results.FirstOrDefault(item => item.IsLastResult);
            if (lastResult != null) lastResult.IsLastResult = false;

            var sameResult = results.FirstOrDefault(item => item.CreateTime == newResult.CreateTime);
            if (sameResult != null) results.Remove(sameResult);

            newResult.IsLastResult = true;
            results.Add(newResult);

            var orderedResults = new List<GameResult>(12);
            if (gameMode == Mode.Classic)
                orderedResults = results.OrderByDescending(model => model.Score).ToList();
            if (gameMode == Mode.Infinity)
                orderedResults = results.OrderBy(model => model.Time).ToList();
            if (gameMode == Mode.TimeAttack)
                orderedResults = results.OrderByDescending(model => model.Score).ToList();
            
            if (orderedResults.Count > 10)
            {
                if(!orderedResults[10].IsLastResult) orderedResults.RemoveAt(10);
                if(orderedResults.Count == 12 && orderedResults[10].IsLastResult) orderedResults.RemoveAt(11);
                if(orderedResults.Count == 12 && orderedResults[11].IsLastResult) orderedResults.RemoveAt(10);
                if(orderedResults.Count == 11 && !orderedResults[10].IsLastResult) orderedResults.RemoveAt(10);
            }

            foreach (var result in orderedResults)
                result.Number = orderedResults.IndexOf(result) + 1;

            await WriteToFile(orderedResults, gameMode);
            return new ObservableCollection<GameResult>(orderedResults);
        }

        public async Task<ObservableCollection<GameResult>> UpdateLastNickNameAsync(ObservableCollection<GameResult> results, string nickName, Mode gameMode)
        {
            var lastResult = results.FirstOrDefault(x => x.IsLastResult);
            if(lastResult !=null)
                lastResult.PlayerName = await GetValidPlayerNameAsync(nickName);
            await WriteToFile(results, gameMode);
            return results;
        } 

        private async Task WriteToFile(IList<GameResult> orderedResults, Mode gameMode)
        {
            var json = JsonSerializeHelper.Serialize(orderedResults);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(gameMode+ ".txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        private async Task<string> GetValidPlayerNameAsync(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName) || string.IsNullOrEmpty(playerName)) playerName = "Player";
            var match = Regex.Match(playerName, @"^.{1,20}$", RegexOptions.IgnoreCase);
            if (!match.Success) playerName = "Player";
            return playerName;
        }
    }
}