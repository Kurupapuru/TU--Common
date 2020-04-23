using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UXK.KurupapuruDialog
{
    [ScriptedImporter(1, new string[]{"kd"})]
    public class KDImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            KDAsset kdAsset;

            using (StreamReader r = new StreamReader(ctx.assetPath))
            {
                kdAsset = KDFIleReader.Read(r);
            }

            var json = JsonConvert.SerializeObject(kdAsset, Formatting.Indented);

            Debug.Log(json);
            
            var strContainer = ScriptableObject.CreateInstance<StringContainer>();
            strContainer.Value = json;
            ctx.AddObjectToAsset("kdAssetJson", strContainer);
             
            foreach (var node in kdAsset.Nodes)
            {
                Debug.Log($"Node: {node.NodeName}");
                foreach (var content in node.Content)
                {
                    Debug.Log(content);
                }
            }
        }
    }

    public class StringContainer : ScriptableObject
    {
        [TextArea]
        public string Value;
    }

    [Serializable]
    public class KDAsset : ScriptableObject
    {
        public List<Actor> Actors = new List<Actor>();
        public List<DialogNode> Nodes = new List<DialogNode>();
    }

    [Serializable]
    public class DialogNode
    {
        public string NodeName;
        public List<DialogContent> Content = new List<DialogContent>();

        public DialogNode(string nodeName)
        {
            NodeName = nodeName;
        }
    }

}