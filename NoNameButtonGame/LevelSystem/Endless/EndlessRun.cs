using System.IO;
using MonoUtils.Logging;

namespace NoNameButtonGame.LevelSystem.Endless;

public class EndlessRun
{
    private double _startTime;
    private double _endTime;

    public bool StartedTimeTracking { get; private set; }
    public bool EndedimeTracking { get; private set; }

    public void StartTimeTracking(double time)
    {
        _startTime = time;
        StartedTimeTracking = true;
    }

    public void EndTimeTracking(double time)
    {
        _endTime = time;
        EndedimeTracking = true;
    }

    public double GetTime()
    {
        if (_endTime == 0 || _startTime == 0)
            return 0;

        return _endTime - _startTime;
    }
}