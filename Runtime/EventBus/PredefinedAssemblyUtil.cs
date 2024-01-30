using System;
using System.Collections.Generic;

namespace Workshop
{
    /// <summary>
    /// Utility class for handling predefined assemblies.
    /// </summary>
    public static class PredefinedAssemblyUtil
    {
        /// <summary>
        /// Enum representing the types of predefined assemblies.
        /// </summary>
        private enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass,
        }

        /// <summary>
        /// Gets the type of the assembly based on its name.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>The type of the assembly, or null if the assembly name does not match any predefined types.</returns>
        private static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null
            };
        }

        /// <summary>
        /// Gets a list of types that implement a specific interface from predefined assemblies.
        /// </summary>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <returns>A list of types that implement the interface.</returns>
        public static List<Type> GetTypes(Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            var types = new List<Type>();

            foreach (var t in assemblies)
            {
                var assemblyType = GetAssemblyType(t.GetName().Name);
                if (assemblyType != null)
                {
                    assemblyTypes.Add((AssemblyType)assemblyType, t.GetTypes());
                }
            }

            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharp], types, interfaceType);
            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharpFirstPass], types, interfaceType);

            return types;
        }

        /// <summary>
        /// Adds types from a specific assembly to a collection of types if they implement a specific interface.
        /// </summary>
        /// <param name="assembly">The assembly to get types from.</param>
        /// <param name="types">The collection of types to add to.</param>
        /// <param name="interfaceType">The type of the interface.</param>
        private static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
        {
            if (assembly == null) return;

            foreach (var t in assembly)
            {
                if (t.IsInterface || t.IsAbstract) continue;
                if (interfaceType.IsAssignableFrom(t))
                {
                    types.Add(t);
                }
            }
        }
    }
}