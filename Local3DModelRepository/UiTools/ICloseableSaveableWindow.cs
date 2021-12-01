using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local3DModelRepository.UiTools
{
    public interface ICloseableSaveableWindow
    {
        ICommand CloseWithoutSavingCommand { get; }

        ICommand CloseAndSaveCommand { get; }
    }
}
