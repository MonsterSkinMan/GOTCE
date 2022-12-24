using System;
using System.Reflection;
using GOTCE;

namespace GOTCE.Misc {
    [AttributeUsage(AttributeTargets.Method)]
     /// <summary>Runs a method after a specified event. Fails if applied to a non-static method.</summary>
    /// <param name="_after">The event to run after, defaults to start</param>
    public class RunMethodAttribute : Attribute {
        public RunAfter after;
        public RunMethodAttribute(RunAfter _after = RunAfter.Start) {
            after = _after;
        }
    }

    public enum RunAfter {
        Skills,
        Items,
        Enemies,
        Misc,
        Start
    }

    public class HooksAttributeLogic {
        private static List<MethodInfo> skills;
        private static List<MethodInfo> start;
        private static List<MethodInfo> enemies;
        private static List<MethodInfo> misc;
        private static List<MethodInfo> items;
        private static BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        public static void Scan() {
            skills = new();
            start = new();
            enemies = new();
            misc = new();
            items = new();
            
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            List<MethodInfo> methods = new();
            foreach (Type type in types) {
                foreach (MethodInfo info in type.GetMethods(flags)) {
                    methods.Add(info);
                }
            }

            foreach (MethodInfo info in methods) {
                if (info.GetCustomAttribute<RunMethodAttribute>() != null) {
                    RunMethodAttribute attr = info.GetCustomAttribute<RunMethodAttribute>();

                    switch (attr.after) {
                        case RunAfter.Skills:
                            skills.Add(info);
                            break;
                        case RunAfter.Items:
                            items.Add(info);
                            break;
                        case RunAfter.Enemies:
                            enemies.Add(info);
                            break;
                        case RunAfter.Misc:
                            misc.Add(info);
                            break;
                        default:
                            start.Add(info);
                            break;
                    }
                }
            }
        }

        public static void CallAttributeMethods(RunAfter after) {
            switch (after) {
                case RunAfter.Skills:
                    CallEach(skills);
                    break;
                case RunAfter.Items:
                    CallEach(items);
                    break;
                case RunAfter.Enemies:
                    CallEach(enemies);
                    break;
                case RunAfter.Misc:
                    CallEach(misc);
                    break;
                default:
                    CallEach(start);
                    break;
            }
        }

        private static void CallEach(List<MethodInfo> methods) {
            BindingFlags flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            foreach (MethodInfo info in methods) {
                if (!info.IsStatic) {
                    Main.ModLogger.LogDebug($"Method {info.Name} attempted to use HooksAttribute but was non-static. Not executing it.");
                }
                info.ReflectedType.InvokeMember(info.Name, flag, null, info.DeclaringType, null);
            }
        }
    }
}