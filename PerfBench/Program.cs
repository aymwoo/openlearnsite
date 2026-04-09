using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        int numRows = 10000;
        int numCids = 5000;

        DataTable dt1 = new DataTable();
        dt1.Columns.Add("Cid");
        dt1.Columns.Add("Checker");

        for (int i = 0; i < numRows; i++)
        {
            dt1.Rows.Add(i.ToString());
        }

        List<string> cidsList = new List<string>();
        for (int i = 0; i < numCids; i++)
        {
            cidsList.Add((i * 2).ToString());
        }
        string Cids = string.Join(",", cidsList);

        DataTable dt2 = dt1.Copy();

        // Warmup
        Baseline(dt1.Copy(), Cids);
        Optimized(dt2.Copy(), Cids);

        // Baseline
        Stopwatch sw = Stopwatch.StartNew();
        for (int i=0; i<10; i++) Baseline(dt1, Cids);
        sw.Stop();
        Console.WriteLine($"Baseline: {sw.ElapsedMilliseconds} ms");

        // Optimized
        sw.Restart();
        for (int i=0; i<10; i++) Optimized(dt2, Cids);
        sw.Stop();
        Console.WriteLine($"Optimized: {sw.ElapsedMilliseconds} ms");

        // Verify Correctness
        bool isCorrect = true;
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            if (dt1.Rows[i]["Checker"].ToString() != dt2.Rows[i]["Checker"].ToString())
            {
                Console.WriteLine($"Mismatch at row {i}");
                isCorrect = false;
                break;
            }
        }
        if (isCorrect) Console.WriteLine("Correctness verified!");
    }

    static void Baseline(DataTable dt, string Cids)
    {
        int dtcount = dt.Rows.Count;
        string[] CidsStr = Cids.Split(',');
        if (CidsStr.Length > 0)
        {
            for (int i = 0; i < dtcount; i++)
            {
                string Cid = dt.Rows[i]["Cid"].ToString();
                foreach (string ch in CidsStr)
                {
                    if (ch == Cid)
                    {
                        dt.Rows[i]["Checker"] = "1";
                        break;
                    }
                }
            }
        }
    }

    static void Optimized(DataTable dt, string Cids)
    {
        int dtcount = dt.Rows.Count;
        string[] CidsStr = Cids.Split(',');
        if (CidsStr.Length > 0)
        {
            System.Collections.Generic.HashSet<string> cidSet = new System.Collections.Generic.HashSet<string>(CidsStr);
            for (int i = 0; i < dtcount; i++)
            {
                string Cid = dt.Rows[i]["Cid"].ToString();
                if (Cid != null && cidSet.Contains(Cid))
                {
                    dt.Rows[i]["Checker"] = "1";
                }
            }
        }
    }
}
