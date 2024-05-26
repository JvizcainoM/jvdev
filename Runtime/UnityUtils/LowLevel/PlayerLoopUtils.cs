using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace JvDev.UnityUtils.LowLevel
{
    public static class PlayerLoopUtils
    {
        public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
        {
            if (loop.type != typeof(T)) return HandleSubSystemLoop<T>(ref loop, systemToInsert, index);

            var playerLoopSystemList = new List<PlayerLoopSystem>();

            if (loop.subSystemList != null)
                playerLoopSystemList.AddRange(loop.subSystemList);

            playerLoopSystemList.Insert(index, systemToInsert);
            loop.subSystemList = playerLoopSystemList.ToArray();
            return true;
        }

        public static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove)
        {
            if (loop.subSystemList == null) return;
            var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);

            for (var i = 0; i < playerLoopSystemList.Count; i++)
            {
                if (playerLoopSystemList[i].type != systemToRemove.type
                    || playerLoopSystemList[i].updateDelegate != systemToRemove.updateDelegate) continue;

                playerLoopSystemList.RemoveAt(i);
                loop.subSystemList = playerLoopSystemList.ToArray();
            }

            HandleSubSystemLoopForRemoval<T>(ref loop, systemToRemove);
        }

        private static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem loopToInsert,
            int index)
        {
            if (loop.subSystemList == null) return false;

            for (var i = 0; i < loop.subSystemList.Length; i++)
            {
                if (!InsertSystem<T>(ref loop.subSystemList[i], loopToInsert, index)) continue;
                return true;
            }

            return false;
        }

        private static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem loop,
            in PlayerLoopSystem loopToRemove)
        {
            if (loop.subSystemList == null) return;

            for (var i = 0; i < loop.subSystemList.Length; i++)
            {
                RemoveSystem<T>(ref loop.subSystemList[i], loopToRemove);
            }
        }

        public static void PrintPlayerLoop(PlayerLoopSystem loopSystem)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Unity PlayerLoopSystem:");
            foreach (var subSystem in loopSystem.subSystemList)
            {
                PrintSubSystem(subSystem, sb, 0);
            }

            Debug.Log(sb.ToString());
        }

        private static void PrintSubSystem(PlayerLoopSystem loopSystem, StringBuilder sb, int depth)
        {
            sb.Append(' ', depth * 2).AppendLine(loopSystem.type.Name);
            if (loopSystem.subSystemList == null || loopSystem.subSystemList.Length == 0) return;


            foreach (var subSubSystem in loopSystem.subSystemList)
            {
                PrintSubSystem(subSubSystem, sb, depth + 1);
            }
        }
    }
}