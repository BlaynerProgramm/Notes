﻿using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Notes.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
	    public AssemblyMappingProfile(Assembly assembly) =>
		    ApplyMappingsFromAssembly(assembly);

		private void ApplyMappingsFromAssembly(Assembly assembly)
		{
			var types = assembly.GetExportedTypes()
				.Where(x => x.GetInterfaces()
					.Any(y => y.IsGenericType && 
					          y.GetGenericTypeDefinition() == typeof(IMapWith<>)))
				.ToList();

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type);
				var methodInfo = type.GetMethod("Mapping");
				methodInfo?.Invoke(instance, new[] { this });
			}
		}
	}
}