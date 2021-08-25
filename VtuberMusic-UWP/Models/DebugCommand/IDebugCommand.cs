using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.DebugCommand
{
    public interface IDebugCommand
    {
        string Title { get; }
        string Description { get; }

        void Do();
    }
}
