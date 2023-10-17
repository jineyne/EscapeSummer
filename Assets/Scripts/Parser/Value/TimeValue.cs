using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시간을 나타내는 값
/// </summary>
public class TimeValue : Value {
    /// <summary>
    /// 하루의 시간
    /// </summary>
    public const int HoursPerHalfDay = 5;

    public const int DaySecondsPerDay = 120;

    public const int HalfDaySecondsPerDay = DaySecondsPerDay / 2;

    /// <summary>
    /// 날자를 반환한다
    /// </summary>
    public int Days {
        get { return value / DaySecondsPerDay; }
    }

    public int DaySeconds {
        get { return value % DaySecondsPerDay; }
    }

    /// <summary>
    /// 시간을 24시간제로 반환한다
    /// </summary>
    public int Hours {
        get { return DaySeconds / 5; }
    }


    /// <summary>
    /// 시간을 12시간제로 반환한다
    /// </summary>
    public int MeridiemHours {
        get { 
            var hours = Hours;
            if (hours > 12) {
                return hours - 12;
            }

            return hours;
        }
    }

    /// <summary>
    /// 오전 오후를 나타내는 문자열을 반환한다
    /// </summary>
    public string Meridiem {
        get { return DaySeconds >= (HalfDaySecondsPerDay) ? "PM" : "AM"; }
    }

    public bool IsDay {
        get { return DaySeconds < (HalfDaySecondsPerDay); }
    }

    public bool IsNight {
        get { return DaySeconds >= (HalfDaySecondsPerDay); }
    }

    public int Value { 
        get { return value; } 
    }

    [SerializeField]
    private int value;

    public override Value Visit(IVirtualMachine machine) {
        return this;
    }

    private List<Value> mAlreadyUseList = new List<Value>();

    public override void Set(Value value) {
        if (mAlreadyUseList.Contains(value)) {
            return;
        }

        int targetTime = 0;
        if (value is TimeValue) {
            targetTime = (value as TimeValue).Value;
        } else {
            return;
        }

        //} else if (value is IntValue) {
        //    targetTime = (value as IntValue).Value;
        //}


        if (targetTime > HalfDaySecondsPerDay) {
            this.value += DaySecondsPerDay - DaySeconds;
        } else {
            if (DaySeconds >= targetTime) {
                this.value += DaySecondsPerDay - (DaySeconds - targetTime);
            } else {
                this.value += Mathf.Max(0, targetTime - DaySeconds);
            }
        }

        mAlreadyUseList.Add(value);
    }

    public override void Add(Value value) {
        int targetTime = 0;
        if (value is TimeValue) {
            targetTime = (value as TimeValue).Value;
        } else if (value is IntValue) {
            targetTime = (value as IntValue).Value;
        }

        this.value += targetTime;
    }

    public override Value Clone(GameObject target) {
        var component = target.AddComponent<TimeValue>();
        component.value = value;
        return component;
    }

    public override string ToString() {
        return string.Format("{0} 일 {1} {2} 시", Days, Meridiem, MeridiemHours);
    }
}
