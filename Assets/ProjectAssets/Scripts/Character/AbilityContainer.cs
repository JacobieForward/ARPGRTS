using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("AbilityList")]
public class AbilityContainer {
    [XmlArray("Abilities")]
    [XmlArrayItem("Ability")]
    public List<Ability> abilities = new List<Ability>();

    // Load an instance of the ability container with all abilities populated from the XML
    public static AbilityContainer Load(string path) {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(AbilityContainer));

        StringReader reader = new StringReader(_xml.text);

        AbilityContainer abilities = serializer.Deserialize(reader) as AbilityContainer;

        reader.Close();

        return abilities;
    }

    
}
