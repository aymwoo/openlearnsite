using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            int cn = 1000;

            // Generate mock DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("Qid", typeof(int));
            for (int i = 0; i < cn; i++)
            {
                dt.Rows.Add(i + 1);
            }

            Console.WriteLine($"Running benchmarks for {cn} rows...\n");

            // Pre-warm GC
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // ==========================================
            // Baseline: Old loop approach (N+1 query)
            // ==========================================
            long memoryBeforeBase = GC.GetTotalMemory(true);
            var swBase = Stopwatch.StartNew();

            int executedBase = 0;

            for (int i = 0; i < cn; i++)
            {
                string Qid = dt.Rows[i][0].ToString();
                int ps = i + 1;
                string sql = "update TurtleQuestion set Qsort= " + ps + " where Qid=" + Qid;
                // Mock execution
                executedBase++;
            }

            swBase.Stop();
            long memoryAfterBase = GC.GetTotalMemory(true);
            long baselineMemory = memoryAfterBase - memoryBeforeBase;

            Console.WriteLine("=== Baseline (Old Approach) ===");
            Console.WriteLine($"Mock Queries Executed: {executedBase}");
            Console.WriteLine($"Time Elapsed: {swBase.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Allocated Memory: {Math.Max(0, baselineMemory)} bytes (mocked execution ignores actual string allocations over time due to GC, but string creations cost CPU/Mem)");
            Console.WriteLine();

            // Pre-warm GC
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // ==========================================
            // Optimized: StringBuilder approach
            // ==========================================
            long memoryBeforeOpt = GC.GetTotalMemory(true);
            var swOpt = Stopwatch.StartNew();

            int executedOpt = 0;

            StringBuilder sbSql = new StringBuilder();
            for (int i = 0; i < cn; i++)
            {
                string Qid = dt.Rows[i][0].ToString();
                int ps = i + 1;
                sbSql.AppendFormat("update TurtleQuestion set Qsort={0} where Qid={1};", ps, Qid);
            }
            string finalQuery = sbSql.ToString();
            // Mock execution
            executedOpt = 1;

            swOpt.Stop();
            long memoryAfterOpt = GC.GetTotalMemory(true);
            long optMemory = memoryAfterOpt - memoryBeforeOpt;

            Console.WriteLine("=== Optimized (StringBuilder) ===");
            Console.WriteLine($"Mock Queries Executed: {executedOpt}");
            Console.WriteLine($"Time Elapsed: {swOpt.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Allocated Memory: {Math.Max(0, optMemory)} bytes");
            Console.WriteLine($"Final query length: {finalQuery.Length} characters");
            Console.WriteLine();
        }
    }
}