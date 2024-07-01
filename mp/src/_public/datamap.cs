#define DATAMAP_H

namespace SourceSharp.mp.src._public
{
    public class datamap
    {
        public const float INVALID_TIME = float.MaxValue * -1.0f;

        public enum _fieldtypes
        {
            FIELD_VOID = 0,         // No type or value
            FIELD_FLOAT,            // Any floating point value
            FIELD_STRING,           // A string ID (return from ALLOC_STRING)
            FIELD_VECTOR,           // Any vector, QAngle, or AngularImpulse
            FIELD_QUATERNION,       // A quaternion
            FIELD_INTEGER,          // Any integer or enum
            FIELD_BOOLEAN,          // boolean, implemented as an int, I may use this as a hint for compression
            FIELD_SHORT,            // 2 byte integer
            FIELD_CHARACTER,        // a byte
            FIELD_COLOR32,          // 8-bit per channel r,g,b,a (32bit color)
            FIELD_EMBEDDED,         // an embedded object with a datadesc, recursively traverse and embedded class/structure based on an additional typedescription
            FIELD_CUSTOM,           // special type that contains function pointers to it's read/write/parse functions

            FIELD_CLASSPTR,         // CBaseEntity *
            FIELD_EHANDLE,          // Entity handle
            FIELD_EDICT,            // edict_t *

            FIELD_POSITION_VECTOR,  // A world coordinate (these are fixed up across level transitions automagically)
            FIELD_TIME,             // a floating point time (these are fixed up automatically too!)
            FIELD_TICK,             // an integer tick count( fixed up similarly to time)
            FIELD_MODELNAME,        // Engine string that is a model name (needs precache)
            FIELD_SOUNDNAME,        // Engine string that is a sound name (needs precache)

            FIELD_INPUT,            // a list of inputed data fields (all derived from CMultiInputVar)
            FIELD_FUNCTION,         // A class function pointer (Think, Use, etc)

            FIELD_VMATRIX,          // a vmatrix (output coords are NOT worldspace)

            // NOTE: Use float arrays for local transformations that don't need to be fixed up.
            FIELD_VMATRIX_WORLDSPACE,// A VMatrix that maps some local space to world space (translation is fixed up on level transitions)
            FIELD_MATRIX3X4_WORLDSPACE, // matrix3x4_t that maps some local space to world space (translation is fixed up on level transitions)

            FIELD_INTERVAL,         // a start and range floating point interval ( e.g., 3.2->3.6 == 3.2 and 0.4 )
            FIELD_MODELINDEX,       // a model index
            FIELD_MATERIALINDEX,    // a material index (using the material precache string table)

            FIELD_VECTOR2D,         // 2 floats

            FIELD_TYPECOUNT,		// MUST BE LAST
        }

        public _fieldtypes fieldtype_t;

        public class CDatamapFieldSizeDeducer
        {
            public _fieldtypes FIELDTYPE;
            public static int FIELDSIZE;

            public CDatamapFieldSizeDeducer(_fieldtypes _fieldType, int _fieldSize)
            {
                FIELDTYPE = _fieldType;
                FIELDSIZE = _fieldSize;
            }
        }

        public static void DECLARE_FIELD_SIZE(_fieldtypes _fieldType, int _fieldSize) => new CDatamapFieldSizeDeducer(_fieldType, _fieldSize);
        public static int FIELD_SIZE(_fieldtypes _fieldType) => CDatamapFieldSizeDeducer.FIELDSIZE;
        public static int FIELD_BITS(_fieldtypes _fieldType) => FIELD_SIZE(_fieldType) * 8;

        // FIXME: Somehow do it similarly to the original file (datamap.h - L#95 to L#125)
        public static void DeclareFields()
        {
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_FLOAT, sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_STRING, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_VECTOR, 3 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_VECTOR2D, 2 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_QUATERNION, 4 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_INTEGER, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_BOOLEAN, sizeof(char));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_SHORT, sizeof(short));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_CHARACTER, sizeof(char));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_COLOR32, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_CLASSPTR, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_EHANDLE, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_EDICT, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_POSITION_VECTOR, 3 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_TIME, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_TICK, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_MODELNAME, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_SOUNDNAME, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_INPUT, sizeof(int));
#if POSIX
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_FUNCTION, sizeof(ulong));
#else
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_FUNCTION, sizeof(int));
#endif // POSIX
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_VMATRIX, 16 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_VMATRIX_WORLDSPACE, 16 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_MATRIX3X4_WORLDSPACE, 12 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_INTERVAL, 2 * sizeof(float));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_MODELINDEX, sizeof(int));
            DECLARE_FIELD_SIZE(_fieldtypes.FIELD_MATERIALINDEX, sizeof(int));
        }

        public unsafe static int ARRAYSIZE2D(int p) => (sizeof(int) / (sizeof(int[][]))); // FIXME: Most likely supposed to mean something else... Figure this out!
        public static int SIZE_OF_ARRAY(int[] p) => p.Length;

        public class _FIELD
        {
            string name;
            _fieldtypes fieldtype;
            int count;
            int flags;
            string mapname;
            int tolerance;

            public _FIELD(_fieldtypes fieldtype = _fieldtypes.FIELD_VOID, string name = null, int count = 0, int flags = 0, string mapname = null, int tolerance = 0)
            {
                this.name = name;
                this.fieldtype = fieldtype;
                this.count = count;
                this.flags = flags;
                this.mapname = mapname;
                this.tolerance = tolerance;
            }
        }

        public static _FIELD DEFINE_FIELD_NULL() => new _FIELD(_fieldtypes.FIELD_VOID, null, 0, 0, null, 0);
        public static _FIELD DEFINE_FIELD(string name, _fieldtypes fieldtype) => new _FIELD(fieldtype, name, 1, FTYPEDESC_SAVE, null, 0);
        public static _FIELD DEFINE_KEYFIELD(string name, _fieldtypes fieldtype, string mapname) => new _FIELD(fieldtype, name, 1, FTYPEDESC_KEY | FTYPEDESC_SAVE, mapname, 0);
        public static _FIELD DEFINE_KEYFIELD_NOT_SAVED(string name, _fieldtypes fieldtype, string mapname) => new _FIELD(fieldtype, name, 1, FTYPEDESC_KEY, mapname, 0);
        public static _FIELD DEFINE_AUTO_ARRAY(string name, _fieldtypes fieldtype) => new _FIELD(fieldtype, name, SIZE_OF_ARRAY(((classNameTypedef)0).name), FTYPEDESC_SAVE, null, 0);
        public static _FIELD DEFINE_AUTO_ARRAY_KEYFIELD(string name, _fieldtypes fieldtype, string mapname) => new _FIELD(fieldtype, name, SIZE_OF_ARRAY(((classNameTypedef)0).name), FTYPEDESC_SAVE, mapname, 0);
        public static _FIELD DEFINE_ARRAY(string name, _fieldtypes fieldtype, int count) => new _FIELD(fieldtype, name, count, FTYPEDESC_SAVE, null, 0);
        //public static _FIELD DEFINE_ENTITY_FIELD(string name, _fieldtypes fieldtype) => new _FIELD() // FIXME: edict_t is to be used here... Somehow?
        // DEFINE_ENTITY_KEYFIELD // FIXME: Ditto as above
        public static _FIELD DEFINE_GLOBAL_FIELD(string name, _fieldtypes fieldtype) => new _FIELD(fieldtype, name, 1, FTYPEDESC_GLOBAL | FTYPEDESC_SAVE, null, 0);
        public static _FIELD DEFINE_GLOBAL_KEYFIELD(string name, _fieldtypes fieldtype, string mapname) => new _FIELD(fieldtype, name, 1, FTYPEDESC_GLOBAL | FTYPEDESC_KEY | FTYPEDESC_SAVE, mapname, 0);
        //public static _FIELD DEFINE_CUSTOM_FIELD(string name, ) // FIXME: ...Gwuh?
        // DEFINE_CUSTOM_KEYFIELD // FIXME: Ditto as above
        public static _FIELD DEFINE_AUTO_ARRAY2D(string name, _fieldtypes fieldtype) => new _FIELD(fieldtype, name, ((bitcount + FIELD_BITS(fieldtype) - 1) & ~(FIELD_BITS(fieldtype) - 1)) / FIELD_BITS(fieldtype));
        public static _FIELD DEFINE_INDEX(string name, _fieldtypes fieldtype) => new _FIELD(fieldtype, name, 1, FTYPEDESC_INDEX, null, 0);

        public static _FIELD DEFINE_EMBEDDED(string name) => new _FIELD(_fieldtypes.FIELD_EMBEDDED,)
    }
}
