using System;
using System.ComponentModel;

public enum UnitType
{
    [Description("%")]
    percent = 0,
    [Description("px")]
    unit = 1
}