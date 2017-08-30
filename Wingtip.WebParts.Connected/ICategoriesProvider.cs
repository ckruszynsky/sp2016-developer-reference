using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wingtip.WebParts.Connected
{
    public interface ICategoriesProvider
    {
        string CategoryId { get; }
    }
}
