using System;
using System.Diagnostics;
using System.Text;

public class Benchmark {
    public static void Main() {
        int cn = 10000;
        string[] YidArray = new string[cn];
        for (int i = 0; i < cn; i++) {
            YidArray[i] = i.ToString();
        }

        Stopwatch sw = Stopwatch.StartNew();
        int dbCalls = 0;
        for (int i = 0; i < cn; i++) {
            string Yid = YidArray[i];
            int ls = i + 1;
            string sql = "update SoftCategory set Ysort= " + ls + " where Yid=" + Yid;
            MockExecuteSql(sql);
            dbCalls++;
        }
        sw.Stop();
        Console.WriteLine($"Baseline: {sw.ElapsedMilliseconds} ms, DB Calls: {dbCalls}");

        sw.Restart();
        dbCalls = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < cn; i++) {
            string Yid = YidArray[i];
            int ls = i + 1;
            sb.Append("update SoftCategory set Ysort=");
            sb.Append(ls);
            sb.Append(" where Yid=");
            sb.Append(Yid);
            sb.Append(";\n");
        }
        MockExecuteSql(sb.ToString());
        dbCalls++;
        sw.Stop();
        Console.WriteLine($"Optimized: {sw.ElapsedMilliseconds} ms, DB Calls: {dbCalls}");
    }

    public static void MockExecuteSql(string sql) {
        // Simulate DB call overhead
        for (int i=0; i<100; i++) {
            int x = i * i;
        }
    }
}
