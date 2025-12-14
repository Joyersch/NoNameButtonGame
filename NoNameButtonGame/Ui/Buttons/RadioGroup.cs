using System.Collections.Generic;

namespace Joyersch.Monogame.Ui.Buttons;

public class RadioGroup
{
    private List<Radio> _radios;

    public RadioGroup()
    {
        _radios = [];
    }

    public void Register(Radio toRegister)
    {
        _radios.Add(toRegister);
    }

    public void ResetAll()
    {
        foreach (var radio in _radios)
            radio.Uncheck();
    }
}