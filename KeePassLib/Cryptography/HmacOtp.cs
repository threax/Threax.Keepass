﻿/*
  KeePass Password Safe - The Open-Source Password Manager
  Copyright (C) 2003-2022 Dominik Reichl <dominik.reichl@t-online.de>

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

#if !KeePassUAP || NETSTANDARD2_0
using System.Security.Cryptography;
#endif

using KeePassLib.Utility;

#if !KeePassLibSD
namespace KeePassLib.Cryptography
{
	/// <summary>
	/// Generate HMAC-based one-time passwords as specified in RFC 4226.
	/// </summary>
	public static class HmacOtp
	{
		private static readonly uint[] g_vDigitsPower = new uint[] { 1,
			10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

		public static string Generate(byte[] pbSecret, ulong uFactor,
			uint uCodeDigits, bool bAddChecksum, int iTruncationOffset)
		{
			return Generate(pbSecret, uFactor, uCodeDigits, bAddChecksum,
				iTruncationOffset, string.Empty);
		}

		internal static string Generate(byte[] pbSecret, ulong uFactor,
			uint uCodeDigits, bool bAddChecksum, int iTruncationOffset,
			string strAlg)
		{
			if(pbSecret == null) { Debug.Assert(false); pbSecret = MemUtil.EmptyByteArray; }
			if(uCodeDigits == 0) { Debug.Assert(false); return string.Empty; }
			if(uCodeDigits > 8) { Debug.Assert(false); uCodeDigits = 8; }

			byte[] pbText = MemUtil.UInt64ToBytes(uFactor);
			Array.Reverse(pbText); // To big-endian

			byte[] pbHash;
			if(strAlg == "HMAC-SHA-256")
			{
				using(HMACSHA256 h = new HMACSHA256(pbSecret))
				{
					pbHash = h.ComputeHash(pbText);
				}
			}
			else if(strAlg == "HMAC-SHA-512")
			{
				using(HMACSHA512 h = new HMACSHA512(pbSecret))
				{
					pbHash = h.ComputeHash(pbText);
				}
			}
			else
			{
				Debug.Assert(string.IsNullOrEmpty(strAlg) || (strAlg == "HMAC-SHA-1"));
				using(HMACSHA1 h = new HMACSHA1(pbSecret))
				{
					pbHash = h.ComputeHash(pbText);
				}
			}

			uint uOffset = (uint)(pbHash[pbHash.Length - 1] & 0xF);
			if((iTruncationOffset >= 0) && (iTruncationOffset < (pbHash.Length - 4)))
				uOffset = (uint)iTruncationOffset;

			uint uBinary = (uint)(((pbHash[uOffset] & 0x7F) << 24) |
				((pbHash[uOffset + 1] & 0xFF) << 16) |
				((pbHash[uOffset + 2] & 0xFF) << 8) |
				(pbHash[uOffset + 3] & 0xFF));

			uint uOtp = (uBinary % g_vDigitsPower[uCodeDigits]);
			if(bAddChecksum)
				uOtp = ((uOtp * 10) + CalculateChecksum(uOtp, uCodeDigits));

			uint uDigits = (bAddChecksum ? (uCodeDigits + 1) : uCodeDigits);
			return uOtp.ToString(NumberFormatInfo.InvariantInfo).PadLeft(
				(int)uDigits, '0');
		}

		private static readonly uint[] g_vDoubleDigits = new uint[] {
			0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };

		private static uint CalculateChecksum(uint uNum, uint uDigits)
		{
			bool bDoubleDigit = true;
			uint uTotal = 0;

			while(0 < uDigits--)
			{
				uint uDigit = (uNum % 10);
				uNum /= 10;

				if(bDoubleDigit) uDigit = g_vDoubleDigits[uDigit];

				uTotal += uDigit;
				bDoubleDigit = !bDoubleDigit;
			}

			uint uResult = (uTotal % 10);
			if(uResult != 0) uResult = 10 - uResult;

			return uResult;
		}

		// RFC 6238
		internal static string GenerateTimeOtp(byte[] pbSecret, DateTime? odt,
			uint uTimeStep, uint uCodeDigits, string strAlg)
		{
			DateTime dt = (odt.HasValue ? TimeUtil.ToUtc(odt.Value, true) :
				DateTime.UtcNow);
			ulong uStep = ((uTimeStep != 0) ? uTimeStep : 30U);
			ulong uTime = (ulong)TimeUtil.SerializeUnix(dt) / uStep;

			return Generate(pbSecret, uTime, uCodeDigits, false, -1, strAlg);
		}
	}
}
#endif
