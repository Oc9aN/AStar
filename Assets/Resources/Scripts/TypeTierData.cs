using System;

[Serializable]
public class TypeTierData
{
    public string[] types; // "type" 필드 (문자열 배열)
    public TierPercent[] tierPercents; // "tierPercent" 필드 (TierPercent 배열)
}

[Serializable]
public class TierPercent
{
    public float[] percents; // "percent" 필드 (정수 배열)
}