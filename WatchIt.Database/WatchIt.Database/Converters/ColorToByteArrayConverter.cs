using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WatchIt.Database.Converters;

public class ColorToByteArrayConverter : ValueConverter<Color, byte[]>
{
    #region CONSTRUCTORS

    public ColorToByteArrayConverter() : base(x => new byte[] { x.R, x.G, x.B, x.A }, x => Color.FromArgb(x.Length > 3 ? x[3] : 0x00, x[0], x[1], x[2])) {}
    
    #endregion
}