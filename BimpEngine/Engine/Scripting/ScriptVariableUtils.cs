using BimpEngine.Engine.Math;
using BimpEngine.Engine.Scripting.Enum;
using System.Globalization;

namespace BimpEngine.Engine.Scripting
{
    public static class ScriptVariableUtils
    {
        public static VariableType? MapClrType(Type tipo)
        {
            if (tipo == typeof(float) || tipo == typeof(double)) return VariableType.Float;
            if (tipo == typeof(int)) return VariableType.Int;
            if (tipo == typeof(bool)) return VariableType.Bool;
            if (tipo == typeof(string)) return VariableType.String;
            if (tipo == typeof(Vector3)) return VariableType.Vector3;
            return null;
        }

        public static string FormatValor(object? valor, VariableType tipo)
        {
            if (valor == null) return "";
            if (tipo == VariableType.Vector3 && valor is Vector3 v)
                return $"{v.X};{v.Y};{v.Z}";
            return Convert.ToString(valor, CultureInfo.InvariantCulture) ?? "";
        }

        public static object? ParseValor(string raw, VariableType tipo)
        {
            if (string.IsNullOrWhiteSpace(raw) && tipo != VariableType.String) return null;

            try
            {
                return tipo switch
                {
                    VariableType.Float => double.Parse(raw, CultureInfo.InvariantCulture),
                    VariableType.Int => int.Parse(raw, CultureInfo.InvariantCulture),
                    VariableType.Bool => bool.Parse(raw),
                    VariableType.String => raw,
                    VariableType.Vector3 => ParseVector3(raw),
                    _ => raw
                };
            }
            catch { return null; }
        }

        private static Vector3 ParseVector3(string raw)
        {
            var partes = raw.Split(';');
            double x = partes.Length > 0 && double.TryParse(partes[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var px) ? px : 0;
            double y = partes.Length > 1 && double.TryParse(partes[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var py) ? py : 0;
            double z = partes.Length > 2 && double.TryParse(partes[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var pz) ? pz : 0;
            return new Vector3(x, y, z);
        }

        public static object? ConvertirParaClr(object? valor, Type tipoDestino)
        {
            if (valor == null) return null;

            if (tipoDestino == typeof(float)) return Convert.ToSingle(valor, CultureInfo.InvariantCulture);
            if (tipoDestino == typeof(double)) return Convert.ToDouble(valor, CultureInfo.InvariantCulture);
            if (tipoDestino == typeof(int)) return Convert.ToInt32(valor, CultureInfo.InvariantCulture);
            if (tipoDestino == typeof(bool)) return Convert.ToBoolean(valor);
            if (tipoDestino == typeof(string)) return valor.ToString();

            return valor;
        }
    }
}
