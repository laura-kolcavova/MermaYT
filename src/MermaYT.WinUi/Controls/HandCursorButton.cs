using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;

namespace MermaYT.WinUi.Controls;

public sealed class HandCursorButton :
    Button
{
    public HandCursorButton()
    {
        ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }
}
