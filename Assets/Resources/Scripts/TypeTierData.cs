using System;

[Serializable]
public class TypeTierData
{
    public string[] type; // "type" 필드 (문자열 배열)
    public TierPercent[] tierPercent; // "tierPercent" 필드 (TierPercent 배열)
}

[Serializable]
public class TierPercent
{
    public float[] percent; // "percent" 필드 (정수 배열)
}