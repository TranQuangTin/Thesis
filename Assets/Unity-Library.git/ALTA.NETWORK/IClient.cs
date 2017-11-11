using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alta.INetwork
{
    [Flags]
    public enum TypeClient
    {
        None = 1, IPAD=2
    }

    public interface IClient
    {
        TypeClient Type { get; }
        string Code { get; }
    }
}
