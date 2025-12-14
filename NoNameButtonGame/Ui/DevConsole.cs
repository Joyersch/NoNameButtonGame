using System.Collections.Generic;
using System.Linq;
using Joyersch.Monogame.Console;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Console;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame.Ui;

public sealed class DevConsole : IManageable
{
    private BasicText _inputDisplay;
    private BasicText _cursorDisplay;
    private BasicText _maxBasicText;
    private BasicText _calculationBasicText;

    private string _input = string.Empty;
    private string _prediction = string.Empty;

    private BasicText[] _lines;

    private Scene _scene;
    private List<BacklogRow> _toDisplay;
    private int _maxLinesY;
    private bool _isDrawingCursor;
    private OverTimeInvoker _drawCursorInvoker;
    private Blank _background;

    private List<string> _priorCommands = [];
    private int _priorPointer;
    private bool _keyUpDown;
    private bool _keyDownDown;
    private bool _keyPageUpDown;
    private bool _keyPageDownDown;

    public Backlog Backlog { get; private set; }

    public IProcessor Processor { get; private set; }

    public ContextProvider Context { get; private set; }

    public Rectangle Rectangle => _scene.Camera.Rectangle;

    public DevConsole(IProcessor processor, Scene scene) : this(processor,
        scene, null)
    {
    }

    public DevConsole(IProcessor processor, Scene scene, DevConsole? console)
    {
        Processor = console is null ? processor : console.Processor;
        _scene = scene;
        _maxBasicText = new BasicText("[block]", scene.Display.Scale * 4);
        _calculationBasicText = new BasicText(string.Empty, scene.Display.Scale * 4);
        _inputDisplay = new BasicText(string.Empty, scene.Display.Scale * 4);

        Backlog = console is null ? new Backlog() : console.Backlog;

        _maxLinesY = (int)(scene.Camera.Size / _maxBasicText.Size).Y - 1;
        _toDisplay = new List<BacklogRow>();

        _background = new Blank(Vector2.Zero, scene.Display.Size.X)
        {
            Color = new Microsoft.Xna.Framework.Color(0, 0, 0, 128)
        };

        _lines = new BasicText[_maxLinesY];
        for (int i = 0; i < _maxLinesY; i++)
        {
            _lines[i] = new BasicText(string.Empty, scene.Display.Scale * 4);
            _lines[i].Move(new Vector2(0, _maxBasicText.Size.Y) * i);
        }

        _inputDisplay.ChangeText(string.Empty);
        _cursorDisplay = new BasicText("_", scene.Display.Scale * 4);

        _drawCursorInvoker = new OverTimeInvoker(500F);
        _drawCursorInvoker.Trigger += UpdateCursor;

        _inputDisplay.Move(new Vector2(0, scene.Camera.Size.Y - _maxBasicText.Size.Y));

        Context = console is null ? new ContextProvider() : console.Context;
    }

    private void UpdateCursor()
    {
        _isDrawingCursor = !_isDrawingCursor;
        _cursorDisplay.ChangeText(_isDrawingCursor ? "_" : "");
    }

    public void Update(GameTime gameTime)
    {
        _drawCursorInvoker.Update(gameTime);
        _background.Update(gameTime);
        _toDisplay = Backlog.GetRangeFromPointer(_maxLinesY);

        for (int line = 0; line < _lines.Length; line++)
        {
            BasicText l = _lines[line];
            l.Move(new Vector2(0, _maxBasicText.Size.Y) * line);
            if (_toDisplay.Count > line)
            {
                var text = _toDisplay[line].Text;
                int i = text.Length;
                do
                {
                    // Quick fix to cut of overlapping lines.
                    // There should be a better solution like a linebreak but that would invoke effort!
                    l.ChangeText(text.Substring(0, i--));
                } while (l.Rectangle.Width > _scene.Camera.Size.X);

                l.ChangeColor(_toDisplay[line].ColorSet.Color);
            }
            else
                l.ChangeText(string.Empty);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_keyUpDown)
        {
            _keyUpDown = true;
            if (_priorCommands.Count > 0)
            {
                _priorPointer++;
                if (_priorPointer >= _priorCommands.Count)
                    _priorPointer = _priorCommands.Count;
           
                _input = _priorCommands[^_priorPointer];
            }
        }

        _keyUpDown = Keyboard.GetState().IsKeyDown(Keys.Up);

        if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_keyDownDown)
        {
            _priorPointer--;
            if (_priorPointer < 0)
                _priorPointer = 0;
            _input = _priorPointer == 0 ? string.Empty : _priorCommands[^_priorPointer];
        }

        _keyDownDown = Keyboard.GetState().IsKeyDown(Keys.Down);

        if (Keyboard.GetState().IsKeyDown(Keys.PageUp) && !_keyPageUpDown)
            Backlog.MovePointerUp();
        _keyPageUpDown = Keyboard.GetState().IsKeyDown(Keys.PageUp);

        if (Keyboard.GetState().IsKeyDown(Keys.PageDown) && !_keyPageDownDown)
            Backlog.MovePointerDown();
        _keyPageDownDown = Keyboard.GetState().IsKeyDown(Keys.PageDown);

        _prediction = Processor.PossibleMatch(_input) ?? string.Empty;
        _inputDisplay.Update(gameTime);
        _inputDisplay.ChangeText(CalculateInput());
        _inputDisplay.ChangeColor(CalculateInputColor());

        _calculationBasicText.ChangeText(_input);
        _cursorDisplay.Move(_inputDisplay.Position + new Vector2(_calculationBasicText.Size.X, 0));
        _cursorDisplay.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _background.Draw(spriteBatch);

        foreach (BasicText BasicText in _lines)
            BasicText.Draw(spriteBatch);
        _inputDisplay.Draw(spriteBatch);
        if (_isDrawingCursor)
            _cursorDisplay.Draw(spriteBatch);
    }

    /// <summary>
    /// Simulates User input to run a specific command.
    /// </summary>
    /// <param name="command"></param>
    public void RunCommand(string command)
    {
        _priorCommands.Add(command);
        Backlog.Add(new BacklogRow(command));
        var output = Processor.Process(this, command, Context).Select(s => new BacklogRow(s))
            .ToArray();
        Backlog.AddRange(output);
        var length = output.Length;

        if (Backlog.Count > _maxLinesY)
        {
            int moveBy = Backlog.Count - _maxLinesY - Backlog.Pointer;
            for (int i = 0; i < moveBy; i++)
                Backlog.MovePointerDown();
        }
    }

    public void TextInput(object sender, TextInputEventArgs e)
    {
        string c = e.Character.ToString();

        switch (e.Character)
        {
            case '\b':
                if (_input.Length > 0)
                    _input = _input[..^1];
                break;
            case (char)27:
                _input = string.Empty;
                break;
            case '\r':
                RunCommand(_input);
                _input = string.Empty;
                _priorPointer = 0;
                break;
            case '\t':
                if (_input.Length < _prediction.Length)
                    _input = CalculateInput();
                break;
            default:
                string possibleInput = CalculateInput() + c + "_";
                _calculationBasicText.ChangeText(possibleInput);
                if (_calculationBasicText.Rectangle.Width < _scene.Camera.Size.X)
                    _input += c;
                break;
        }
    }

    private string CalculateInput()
    {
        string match = _prediction;

        if (match.Length < _input.Length)
            return _input;

        return _input + match[_input.Length..];
    }

    private Microsoft.Xna.Framework.Color[] CalculateInputColor()
    {
        Microsoft.Xna.Framework.Color[] whites = new Microsoft.Xna.Framework.Color[_input.Length];
        int grayCount = _prediction.Length - _input.Length;

        if (grayCount < 0)
            grayCount = 0;

        Microsoft.Xna.Framework.Color[] grays = new Microsoft.Xna.Framework.Color[grayCount];

        for (int i = 0; i < whites.Length; i++)
            whites[i] = Microsoft.Xna.Framework.Color.White;
        for (int i = 0; i < grays.Length; i++)
            grays[i] = Microsoft.Xna.Framework.Color.Gray;

        return whites.Concat(grays).ToArray();
    }

    public void Write(string text, int line = -1)
    {
        if (line == -1 || Backlog.Count <= line)
        {
            Backlog.Add(new BacklogRow(text));
            if (Backlog.Count > _maxLinesY)
                Backlog.MovePointerDown();
        }
        else
            Backlog[line].SetText(text);
    }

    public void WriteColor(string text, BacklogColorSet color, int line = -1)
    {
        if (line == -1 || Backlog.Count <= line)
        {
            Backlog.Add(new BacklogRow(text, color));
            if (Backlog.Count > _maxLinesY)
                Backlog.MovePointerDown();
        }
        else
        {
            Backlog[line].SetText(text);
            Backlog[line].SetColor(color);
        }
    }
}