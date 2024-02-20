using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Threading;
using MonoUtils.Ui;
using MonoUtils.Ui.Menu;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.Bonus;

public class Level : SampleLevel
{

    private enum ViewState
    {
        Overworld,
        Ui
    }

    private enum UpdateState
    {
        Overworld,
        Interface
    }

    private readonly string[] _objectives = new[]
    {
        "Objective: Find the castle!",
        "Objective: Enter the castle!",
        "Objective: Help the royal!"
    };


    public Level(Display display, Vector2 window, GameWindow gameWindow, Random random) : base(display, window, random)
    {

    }

}