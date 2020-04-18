using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UXK.KurupapuruDialog
{
    [ScriptedImporter(1, new string[]{"kd", "kd.txt"})]
    public class KDImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            KDAsset kdAsset;

            using (StreamReader r = new StreamReader(ctx.assetPath))
            {
                kdAsset = KDFIleReader.Read(r);
            }
            
            ctx.AddObjectToAsset("kdAsset", kdAsset);
        }
    }

    [Serializable]
    public class KDAsset : ScriptableObject
    {
        public List<Actor> actors = new List<Actor>();
        public List<DialogNode> nodes = new List<DialogNode>();
    }

    [Serializable]
    public class DialogNode
    {
        public List<DialogContent> content;
    }

}