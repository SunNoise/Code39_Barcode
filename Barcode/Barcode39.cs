using System;
using System.Net;

namespace Barcode
{
    public class Barcode39
    {
        public string DrawCode39Barcode(string data, int checkDigit)
        {
            return DrawHTMLBarcode_Code39(data, checkDigit, "yes", "in", 0, 3, 1, 3, "bottom", "center", "", "black", "white");
        }

        private string DrawHTMLBarcode_Code39(string data,
                            int checkDigit,
                            string humanReadable,
                            string units,
                            double minBarWidth,
                            double width, double height,
                            int barWidthRatio,
                            string textLocation,
                            string textAlignment,
                            string textStyle,
                            string foreColor,
                            string backColor)
        {
            return DrawBarcode_Code39(data,
                         checkDigit,
                         humanReadable,
                         units,
                         minBarWidth,
                         width, height,
                         barWidthRatio,
                         textLocation,
                         textAlignment,
                         textStyle,
                         foreColor,
                         backColor,
                         "html");
        }

        private string  DrawBarcode_Code39(string data,
                        int checkDigit,
                        string humanReadable,
                        string units,
                        double minBarWidth,
                        double width, double height,
                        int barWidthRatio,
                        string textLocation,
                        string textAlignment,
                        string textStyle,
                        string foreColor,
                        string backColor,
                        string mode)
        {

            if (String.IsNullOrEmpty(foreColor))
                foreColor = "black";
            if (String.IsNullOrEmpty(backColor))
                backColor = "white";

            if (String.IsNullOrEmpty(textLocation))
                textLocation = "bottom";
            else if (textLocation != "bottom" && textLocation != "top")
                textLocation = "bottom";
            if (String.IsNullOrEmpty(textAlignment))
                textAlignment = "center";
            else if (textAlignment != "center" && textAlignment != "left" && textAlignment != "right")
                textAlignment = "center";
            if (String.IsNullOrEmpty(textStyle))
                textStyle = "";
            if (barWidthRatio == 0)
                barWidthRatio = 3;
            if (height == 0)
                height = 1;
            else if (height <= 0 || height > 15)
                height = 1;
            if (width == 0)
                width = 3;
            else if (width <= 0 || width > 15)
                width = 3;
            else if (minBarWidth < 0 || minBarWidth > 2)
                minBarWidth = 0;
            if (String.IsNullOrEmpty(units))
                units = "in";
            else if (units != "in" && units != "cm")
                units = "in";
            if (String.IsNullOrEmpty(humanReadable))
                humanReadable = "yes";
            else if (humanReadable != "yes" && humanReadable != "no")
                humanReadable = "yes";

            var encodedData = EncodeCode39(data, checkDigit);
            var humanReadableText = ConnectCode_Encode_Code39(data, checkDigit);
            var encodedLength = 0;
            var thinLength = 0;
            var thickLength = 0.0;
            var totalLength = 0.0;
            var incrementWidth = 0.0;
            var swing = 1;
            var result = "";
            var barWidth = 0.0;
            var thickWidth = 0.0;
            if (barWidthRatio >= 2 && barWidthRatio <= 3)
            {
            }
            else
                barWidthRatio = 3;

            for (int x = 0; x < encodedData.Length; x++)
            {
                if (encodedData[x] == 't')
                {
                    thinLength++;
                    encodedLength++;
                }
                else if (encodedData[x] == 'w')
                {
                    thickLength = thickLength + barWidthRatio;
                    encodedLength = encodedLength + 3;
                }
            }
            totalLength = totalLength + thinLength + thickLength;

            if (minBarWidth > 0)
            {
                barWidth = Math.Round(minBarWidth,2);
                width = barWidth * totalLength;
            }
            else
                barWidth = Math.Round(width / totalLength,2);

            thickWidth = barWidth * 3;
            if (barWidthRatio >= 2 && barWidthRatio <= 3.0)
            {
                thickWidth = barWidth * barWidthRatio;
            }

            if (mode == "html")
            {
                if (textAlignment == "center")
                    result = "<div style=\"text-align:center\">";
                else if (textAlignment == "left")
                    result = "<div style=\"text-align:left;\">";
                else if (textAlignment == "right")
                    result = "<div style=\"text-align:right;\">";

                var humanSpan = "";
                if (humanReadable == "yes" && textLocation == "top")
                {
                    if (String.IsNullOrEmpty(textStyle))
                        humanSpan = "<span style=\"font-family : arial; font-size:12pt\">" + humanReadableText + "</span><br />";
                    else
                        humanSpan = "<span style=" + textStyle + ">" + humanReadableText + "</span><br />";
                }
                result = result + humanSpan;
            }

            for (int x = 0; x < encodedData.Length; x++)
            {
                string brush;
                if (swing == 0)
                    brush = backColor;
                else
                    brush = foreColor;

                if (encodedData[x] == 't')
                {
                    if (mode == "html")
                        result = result
                             + "<span style=\"border-left:"
                             + barWidth
                             + units
                             + " solid "
                             + brush
                             + ";height:"
                             + height
                             + units + ";display:inline-block;\"></span>";
                    incrementWidth = incrementWidth + barWidth;
                }
                else if (encodedData[x] == 'w')
                {
                    if (mode == "html")
                        result = result
                             + "<span style=\"border-left :"
                             + thickWidth
                             + units + " solid "
                             + brush
                             + ";height:"
                             + height
                             + units + ";display:inline-block;\"></span>";
                    incrementWidth = incrementWidth + thickWidth;
                }

                if (swing == 0)
                    swing = 1;
                else
                    swing = 0;
            }

            if (mode == "html")
            {
                var humanSpan = "";
                if (humanReadable == "yes" && textLocation == "bottom")
                {
                    if (String.IsNullOrEmpty(textStyle))
                        humanSpan = "<br /><span style=\"font-family : arial; font-size:12pt\">" + humanReadableText + "</span>";
                    else
                        humanSpan = "<br /><span style=" + textStyle + ">" + humanReadableText + @"</span>";
                }
                result = result + humanSpan + "</div>";
            }
            return result;
        }

        private string EncodeCode39(string data, int checkDigit)
        {
            var fontOutput = ConnectCode_Encode_Code39(data, checkDigit);
            var output = "";
            var pattern = "";
            for (int x = 0; x < fontOutput.Length; x++)
            {
                switch (fontOutput[x])
                {
                    case '1':
                        pattern = "wttwttttwt";
                        break;
                    case '2':
                        pattern = "ttwwttttwt";
                        break;
                    case '3':
                        pattern = "wtwwtttttt";
                        break;
                    case '4':
                        pattern = "tttwwtttwt";
                        break;
                    case '5':
                        pattern = "wttwwttttt";
                        break;
                    case '6':
                        pattern = "ttwwwttttt";
                        break;
                    case '7':
                        pattern = "tttwttwtwt";
                        break;
                    case '8':
                        pattern = "wttwttwttt";
                        break;
                    case '9':
                        pattern = "ttwwttwttt";
                        break;
                    case '0':
                        pattern = "tttwwtwttt";
                        break;
                    case 'A':
                        pattern = "wttttwttwt";
                        break;
                    case 'B':
                        pattern = "ttwttwttwt";
                        break;
                    case 'C':
                        pattern = "wtwttwtttt";
                        break;
                    case 'D':
                        pattern = "ttttwwttwt";
                        break;
                    case 'E':
                        pattern = "wtttwwtttt";
                        break;
                    case 'F':
                        pattern = "ttwtwwtttt";
                        break;
                    case 'G':
                        pattern = "tttttwwtwt";
                        break;
                    case 'H':
                        pattern = "wttttwwttt";
                        break;
                    case 'I':
                        pattern = "ttwttwwttt";
                        break;
                    case 'J':
                        pattern = "ttttwwwttt";
                        break;
                    case 'K':
                        pattern = "wttttttwwt";
                        break;
                    case 'L':
                        pattern = "ttwttttwwt";
                        break;
                    case 'M':
                        pattern = "wtwttttwtt";
                        break;
                    case 'N':
                        pattern = "ttttwttwwt";
                        break;
                    case 'O':
                        pattern = "wtttwttwtt";
                        break;
                    case 'P':
                        pattern = "ttwtwttwtt";
                        break;
                    case 'Q':
                        pattern = "ttttttwwwt";
                        break;
                    case 'R':
                        pattern = "wtttttwwtt";
                        break;
                    case 'S':
                        pattern = "ttwtttwwtt";
                        break;
                    case 'T':
                        pattern = "ttttwtwwtt";
                        break;
                    case 'U':
                        pattern = "wwttttttwt";
                        break;
                    case 'V':
                        pattern = "twwtttttwt";
                        break;
                    case 'W':
                        pattern = "wwwttttttt";
                        break;
                    case 'X':
                        pattern = "twttwtttwt";
                        break;
                    case 'Y':
                        pattern = "wwttwttttt";
                        break;
                    case 'Z':
                        pattern = "twwtwttttt";
                        break;
                    case '-':
                        pattern = "twttttwtwt";
                        break;
                    case '.':
                        pattern = "wwttttwttt";
                        break;
                    case ' ':
                        pattern = "twwtttwttt";
                        break;
                    case '*':
                        pattern = "twttwtwttt";
                        break;
                    case '$':
                        pattern = "twtwtwtttt";
                        break;
                    case '/':
                        pattern = "twtwtttwtt";
                        break;
                    case '+':
                        pattern = "twtttwtwtt";
                        break;
                    case '%':
                        pattern = "tttwtwtwtt";
                        break;
                    default: break;
                }
                output = output + pattern;
            }
            return output;
        }

        private string ConnectCode_Encode_Code39(string data, int checkDigit)
        {
            var Result = "";
            var cd = "";
            var filtereddata = "";
            filtereddata = filterInput(data);
            var filteredlength = filtereddata.Length;
            if (checkDigit == 1)
            {
                if (filteredlength > 254)
                {
                    filtereddata = filtereddata.Substring(0, 254);
                }
                cd = generateCheckDigit(filtereddata);
            }
            else
            {
                if (filteredlength > 255)
                {
                    filtereddata = filtereddata.Substring(0, 255);
                }
            }
            Result = "*" + filtereddata + cd + "*";
            Result = html_decode(html_escape(Result));
            return Result;
        }

        private char getCode39Character(int inputdecimal)
        {
            var CODE39MAP = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                            'U', 'V', 'W', 'X', 'Y', 'Z', '-', '.', ' ', '$',
                            '/', '+', '%' };
            return CODE39MAP[inputdecimal];
        }

        private int getCode39Value(char inputchar)
        {
            var CODE39MAP = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                            'U', 'V', 'W', 'X', 'Y', 'Z', '-', '.', ' ', '$',
                            '/', '+', '%' };
            var RVal = -1;
            for (int i = 0; i < 43; i++)
            {
                if (inputchar == CODE39MAP[i])
                {
                    RVal = i;
                }
            }
            return RVal;
        }

        private string filterInput(string data)
        {
            var Result = "";
            var datalength = data.Length;
            for (int x = 0; x < datalength; x++)
            {
                if (getCode39Value(data[x]) != -1)
                {
                    Result = Result + data[x];
                }
            }
            return Result;
        }

        private string generateCheckDigit(string data)
        {
            var datalength = data.Length;
            var sumValue = 0;
            for (int x = 0; x < datalength; x++)
            {
                sumValue = sumValue + getCode39Value(data[x]);
            }
            sumValue = sumValue % 43;
            return getCode39Character(sumValue).ToString();
        }

        private string html_escape(string data)
        {
            var Result = "";
            for (int x = 0; x < data.Length; x++)
            {
                Result += $"&#{(int)data[x]};";
            }
            return Result;
        }

        private string html_decode(string str)
        {
            return WebUtility.HtmlDecode(str);
        }
    }
}
