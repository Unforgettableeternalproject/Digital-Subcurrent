using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent 
{
    [System.Serializable]
    public class Dialogue
    {
        [TextArea(3, 10)]
        public string[] sentences;

        public bool showCutscene;

        // �غc��
        public Dialogue(string[] sentences, bool showCutscene)
        {
            this.sentences = sentences;
            this.showCutscene = showCutscene;
        }
    }
}