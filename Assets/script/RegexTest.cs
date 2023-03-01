using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class RegexTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string str = "{'t': 5.46, 'z': 64.50, 'y': -138.02, 'p': 3.89, 'c': -480.00}";
        StringToFloat(str);

    }
    public void StringToFloat(string strInput)
    {
        Regex reg = new Regex(@"[\-\d\.]+");
        MatchCollection mc = reg.Matches(strInput);//设定要查找的字符串
        for (int i = 0; i < mc.Count; i++)
        {
            double d = double.Parse(mc[i].Value);
            Debug.Log(d + "类型为" + d.GetType());
        }
    }
}
