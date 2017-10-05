using System;

namespace Tetris.Common
{
    public class SuspensionManagerException : Exception
    {
        public SuspensionManagerException() { }
        public SuspensionManagerException(Exception e) : base("SuspensionManager failed", e) { }
    }
}