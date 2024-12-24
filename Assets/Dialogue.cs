using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent 
{
    [System.Serializable]
    public class Dialogue
    {
        public string name;

        [TextArea(3, 10)]
        public string[] sentences;

        // �غc��
        public Dialogue(string name, string[] sentences, bool isLeft)
        {
            this.name = name;
            this.sentences = sentences;
        }
    }
}