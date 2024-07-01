#define PLUGINVARIANT_H

namespace SourceSharp.sp.src._public.game.server
{
    class pluginvariant
    {
        public bool bVal;
        public int iVal;
        public float flVal;
        public float[] vecVal = new float[3];
        public color32 rgbaVal;

        edict.edict_t eVal;

        string iszVal;
        fieldtype_t fieldType;

        private pluginvariant()
        {
            fieldType = fieldtype_t.FIELD_VOID;
            iVal = 0;
        }

        private bool Bool() { return fieldType == fieldtype_t.FIELD_BOOLEAN ? bVal : false; }
        private string String() { return ToString(); }
        private int Int() { return fieldType == fieldtype_t.FIELD_INTEGER ? iVal : 0; }
        private float Float() { return fieldType == fieldtype_t.FIELD_FLOAT ? flVal : 0; }
        //private edict.edict_t Edict() { return eVal; }
        private color32 Color32() { return rgbaVal; }
        //private void Vector3D(vector.Vector vec);

        private fieldtype_t FieldType() { return fieldType; }

        private void SetBool(bool b) { bVal = b; fieldType = fieldtype_t.FIELD_BOOLEAN; }
        private void SetString(string str) { iszVal = str; fieldType = fieldtype_t.FIELD_STRING; } // FIXME: Q_snprintf(iszVal, 1024, "%s", str)
        private void SetInt(int val) { iVal = val; fieldType = fieldtype_t.FIELD_INTEGER; }
        private void SetFloat(float val) { flVal = val; fieldType = fieldtype_t.FIELD_FLOAT; }
        private void SetEdict(edict.edict_t val) { eVal = val; fieldType = fieldtype_t.FIELD_EHANDLE; }
        private void SetVector3D(vector.Vector val) { vecVal[0] = val[0]; vecVal[1] = val[1]; vecVal[2] = val[2]; fieldType = fieldtype_t.FIELD_VECTOR; }
        private void SetPositionVector3D(vector.Vector val) { vecVal[0] = val[0]; vecVal[1] = val[1]; vecVal[2] = val[2]; fieldType = fieldtype_t.FIELD_POSITION_VECTOR; }
        private void SetColor32(color32 val) { rgbaVal = val; fieldType = fieldtype_t.FIELD_COLOR32; }
        private void SetColor32(float r, float g, float b, float a) { rgbaVal.r = (byte)r; rgbaVal.g = (byte)g; rgbaVal.b = (byte)b; rgbaVal.a = (byte)a; fieldType = fieldtype_t.FIELD_COLOR32; }

        protected new string ToString()
        {
            string szBuf;

            switch (fieldType)
            {
                case fieldtype_t.FIELD_STRING:
                    return iszVal;

                case fieldtype_t.FIELD_BOOLEAN:
                    if (!bVal)
                    {
                        szBuf = "false";
                    }
                    else
                    {
                        szBuf = "true";
                    }

                    return szBuf;

                case fieldtype_t.FIELD_INTEGER:
                    szBuf = iVal.ToString();
                    return szBuf;

                case fieldtype_t.FIELD_FLOAT:
                    szBuf = flVal.ToString();
                    return szBuf;

                case fieldtype_t.FIELD_COLOR32:
                    szBuf = $"{rgbaVal.r} {rgbaVal.g} {rgbaVal.b} {rgbaVal.a}";
                    return szBuf;

                case fieldtype_t.FIELD_VECTOR:
                    szBuf = $"[{vecVal[0]} {vecVal[1]} {vecVal[2]}]";
                    return szBuf;

                case fieldtype_t.FIELD_VOID:
                    szBuf = "\0";
                    return szBuf;
            }

            return "No conversion to string";
        }

        public void Vector3D(vector.Vector vec)
        {
            if ((fieldType == fieldtype_t.FIELD_VECTOR) || (fieldType == fieldtype_t.FIELD_POSITION_VECTOR))
            {
                vec[0] = vecVal[0];
                vec[1] = vecVal[1];
                vec[2] = vecVal[2];
            }
            else
            {
                vec = vector.vec3_origin;
            }
        }

        public edict.edict_t Edict()
        {
            if (fieldType == fieldtype_t.FIELD_EHANDLE)
            {
                return eVal;
            }

            return null;
        }
    }
}
