// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HsvColor.cs -- color in HSV colorspace.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// Color in HSV colorspace.
    /// </summary>
    [PublicAPI]
    public struct HsvColor
    {
        #region Constants

        /// <summary>
        /// Minimum value of <see cref="HsvColor"/> component.
        /// </summary>
        public const float MinComponentValue = 0f;

        /// <summary>
        /// Maximum value of <see cref="HsvColor"/> component.
        /// </summary>
        public const float MaxComponentValue = 1f;

        #endregion

        #region Properties

        private float _h;

        /// <summary>
        /// Gets or sets H component value of the <see cref="HsvColor"/>.
        /// </summary>
        /// <value>H component value.</value>
        public float H
        {
            [DebuggerStepThrough]
            get
            {
                return _h;
            }
            set
            {
                _CheckComponent(value, "H");
                _h = value;
            }
        }

        private float _s;

        /// <summary>
        /// Gets or sets S component value of the <see cref="HsvColor"/>.
        /// </summary>
        /// <value>S component value.</value>
        public float S
        {
            [DebuggerStepThrough]
            get
            {
                return _s;
            }
            set
            {
                _CheckComponent(value, "S");
                _s = value;
            }
        }

        private float _v;

        /// <summary>
        /// Gets or sets V component value of the <see cref="HsvColor"/>.
        /// </summary>
        /// <value>V component value.</value>
        public float V
        {
            [DebuggerStepThrough]
            get
            {
                return _v;
            }
            set
            {
                _CheckComponent(value, "V");
                _v = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HsvColor"/> class.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="s">The s.</param>
        /// <param name="v">The l.</param>
        public HsvColor(float h, float s, float v)
        {
            _CheckComponent(h, "H");
            _CheckComponent(s, "S");
            _CheckComponent(v, "V");
            _h = h;
            _s = s;
            _v = v;
        }

        #endregion

        #region Private members

        private static void _CheckComponent(float value, string name)
        {
            if ((value < MinComponentValue)
                 || (value > MaxComponentValue))
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// <c>true</c> if obj and this instance are the same type 
        /// and represent the same value; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals
            (
                object obj
            )
        {
            if (!(obj is HsvColor))
            {
                return false; 
            }
            HsvColor other = (HsvColor)obj;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            return (H == other.H) && (S == other.S) && (V == other.V); //-V3024
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            int result = H.GetHashCode();
            result = 29 * result + S.GetHashCode();
            result = 29 * result + V.GetHashCode();
            return result;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents 
        /// the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents 
        /// the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("H: {0}; S: {1}; V: {2}", H, S, V);
        }

        #endregion
    }
}