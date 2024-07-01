// System / Misc. Namespaces
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Runtime.InteropServices;

// C++ Library Namespaces
global using SourceSharp.lib.libc;

// SourceSharp SP
global using SourceSharp.sp.src.common;
global using SourceSharp.sp.src._public;
global using SourceSharp.sp.src._public.zip;
global using SourceSharp.sp.src._public.steam;
global using SourceSharp.sp.src._public.mathlib;
global using SourceSharp.sp.src._public.tier0;

// SourceSharp MP
global using SourceSharp.mp.src.common;
global using SourceSharp.mp.src._public;
global using SourceSharp.mp.src._public.vstdlib;
global using SourceSharp.mp.src._public.tier0;
global using SourceSharp.mp.src._public.tier1;

// Important, Global Variables
global using CBaseEntity = C_BaseEntity; // Originally defined in ...
global using SINGLE_INHERITANCE = CBaseEntity; // Originally defined in mp/src/_public/datamap.cs (datamap.h)
global using color32 = SourceSharp.mp.src._public.tier0.basetypes.color32_s; // Originally defined in mp/src/_public/tier0/basetypes.cs (basetypes.h)
global using fieldtype_t = SourceSharp.mp.src._public.datamap._fieldtypes; // Originally defined in sp/src/_public/game/server/pluginvariant.cs (pluginvariant.h)
global using vec_t = float; // Originally defined in sp/src/_public/mathlib/vector.cs (vector.h)