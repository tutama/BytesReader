


// See https://aka.ms/new-console-template for more information
using System.Globalization;

Console.WriteLine("Translate Hex Data");

//string strRawData = "020909060000010000FF090C07E7020F03031D1000FE20001001E01100090C000003FE07020000008000FF090C00000AFE07030000008000FF0F3C03001601";
string strRawData = "020209060000830008FF02090300030103000420FFFFFFFE0420FFFFFFF9111E110A1101111E";

translate(strRawData, "");


void translate(string strRawData, string indentation) {
    try
    {

        byte[] bytesData = Enumerable.Range(0, strRawData.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(strRawData.Substring(x, 2), 16))
                            .ToArray();

        string strTag = Enum.GetName(typeof(TagDescription), bytesData[0]);
        string strValue;
        int len = 0;

        
        len = bytesData[1];
        strValue = strRawData.Substring(4, len * 2);

        Console.WriteLine();
        Console.WriteLine(indentation + "Structure Data = " + strRawData);
        Console.WriteLine(indentation + "tag = " + strTag);
        //Console.WriteLine(indentation + "len = " + len);
        Console.WriteLine(indentation + "strValue = " + strValue);

        if (strTag == "Structure")
        {

            strValue = strRawData.Substring(4, len * 2);
            string strRawData2 = strRawData.Substring(4);
            for (int i = 0; i < len; i++)
            {

                int len2 = 0;
                bytesData = Enumerable.Range(0, strRawData2.Length)
                                .Where(x => x % 2 == 0)
                                .Select(x => Convert.ToByte(strRawData2.Substring(x, 2), 16))
                                .ToArray();
                strTag = Enum.GetName(typeof(TagDescription), bytesData[0]);

                if (strTag == "Boolean")
                {
                    len2 = 0;
                    if (bytesData[1] == 0)
                    {
                        strValue = "False";
                    }
                    else
                    {
                        strValue = "True";
                    }
                }
                else
                if (strTag == "Integer" || strTag == "Enum" || strTag == "Unsigned")
                {
                    len2 = 0;
                    strValue = bytesData[1].ToString(new CultureInfo("en-us"));
                }
                else
                if (strTag == "Long")
                {
                    len2 = 1;
                    string s = strRawData2.Substring(2, 4);
                    long n = Int64.Parse(s, System.Globalization.NumberStyles.HexNumber);
                    strValue = n.ToString(new CultureInfo("en-us"));
                }
                else
                if (strTag == "BitString")
                {
                    len2 = 4;
                    // Returns -1
                    long longValue = Convert.ToInt64(strRawData2.Substring(4,8), 16);

                    // Returns 1111111111111111111111111111111111111111111111111111111111111111
                    strValue = Convert.ToString(longValue, 2);
                }
                else
                {
                    len2 = bytesData[1];
                    strValue = strRawData2.Substring(4, len2 * 2);

                }


                if (strTag == "Structure")
                {
                    translate(strRawData2, indentation + "   ");
                    return;
                } else
                if (i < len - 1)
                {
                    if (strTag == "Boolean" || strTag == "Integer" || strTag == "Enum" || strTag == "Unsigned")
                    {
                        strRawData2 = strRawData2.Substring(4);
                    }
                    else
                    {
                        strRawData2 = strRawData2.Substring(4 + (len2*2));
                    }
                }

                Console.WriteLine(indentation + "   tag = " + strTag);
                //Console.WriteLine(indentation + "   len = " + len2);
                Console.WriteLine(indentation + "   strValue = " + strValue);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Error = " + e.Message);
    }
}



enum TagDescription {
		NullData = 0,
		Array = 1,
		Structure = 2,
		Boolean = 3,
		BitString = 4,
		DoubleLong = 5,
		DoubleLongUnsigned = 6,
		OctetString = 9,
		VisibleString = 10,
		UTF8String = 12,
		BCD = 13,
		Integer = 15,
		Long = 16,
		Unsigned = 17,
		LongUnsigned = 18,
		CompactArray = 19,
		Long64 = 20,
		Long64Unsigned = 21,
		Enum = 22,
		Float = 23,
		Double = 24,
		DateTime = 25,
		Date = 26,
		Time = 27
	}