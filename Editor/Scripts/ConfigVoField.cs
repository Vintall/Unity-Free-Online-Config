﻿using System;
using Unity_Free_Online_Config.Editor.Enums;
using UnityEngine.Serialization;

namespace Unity_Free_Online_Config.Editor.Scripts
{
    [Serializable]
    public class ConfigVoField
    {
        public EFieldType FieldType;
        public string FieldName;
    }
}