using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPAddress
{
    private int m_part1;
    private int m_part2;
    private int m_part3;
    private int m_part4;

    public static IPAddress Build(int part1, int part2, int part3, int part4)
    {
        return new IPAddress(part1, part2, part3, part4);
    }

    public static IPAddress BuildRandom()
    {
        int part1 = Random.Range(0, 255);
        int part2 = Random.Range(0, 255);
        int part3 = Random.Range(0, 255);
        int part4 = Random.Range(0, 255);

        return Build(part1, part2, part3, part4);
    }

    private IPAddress(int part1, int part2, int part3, int part4)
    {
        m_part1 = part1;
        m_part2 = part2;
        m_part3 = part3;
        m_part4 = part4;
    }

    public override string ToString()
    {
        return $"{m_part1}.{m_part2}.{m_part3}.{m_part4}";
    }
}
