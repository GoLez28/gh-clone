using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Upbeat {
    class Store {
        private static string path;
        private static string fullDirectory;
        private static Type StoreType;

        public static void Init(Type storeType) {
            StoreType = storeType;
            path = GetPath();
            fullDirectory = Path.GetDirectoryName(path);

            if(fullDirectory.Length > 0)
                Directory.CreateDirectory(fullDirectory);

            // get existing properties from this store
            var configs = GetProperties();

            if (!File.Exists(path)) {
                // update with non existing configs
                WriteNewFile(configs);
                return;
            }

            var file_configs = GetFileProperties((k, v) => {
                SaveToStore(k, v);
            });

            if (file_configs.Count > configs.Count) {
                WriteNewFile(configs);
                return;
            }

            // update with non existing configs
            using (StreamWriter sw = File.AppendText(path)) {
                foreach (KeyValuePair<string, string> config in configs) {
                    if (file_configs.ContainsKey(config.Key))
                        continue;
                    sw.WriteLine(config.Key + "=" + config.Value);
                }
            }
        }

        private static void WriteNewFile(Dictionary<string, string> configs) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
            using (FileStream fs = File.OpenWrite(path)) {
                foreach (KeyValuePair<string, string> config in configs) {
                    Byte[] Text = new UTF8Encoding(true).GetBytes(config.Key + "=" + config.Value + Environment.NewLine);
                    fs.Write(Text, 0, Text.Length);
                }
            }
        }

        private static Dictionary<string, string> GetProperties() {
            Dictionary<string, string> configs = new Dictionary<string, string>();
            PropertyInfo[] properties = StoreType.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo property in properties) {
                configs.Add(property.Name, property.GetValue(null, null).ToString());
            }
            return configs;
        }

        private static Dictionary<string, string> GetFileProperties(Action<string, string> callback) {
            Dictionary<string, string> file_configs = new Dictionary<string, string>();
            ReadFile(line => {
                // read existing configs from file
                if (line.Length == 0)
                    return;
                string[] parts = line.Split('=');
                if (parts.Length != 2)
                    return;
                if (file_configs.ContainsKey(parts[0])) {
                    file_configs[parts[0]] = parts[1];
                } else {
                    file_configs.Add(parts[0], parts[1]);
                }
                callback(parts[0], parts[1]);
            });

            return file_configs;
        }

        private static void SaveToStore(string key, string value) {
            // set configs from file
            FieldInfo field = StoreType.GetField("_" + key, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
                return;

            var converter = TypeDescriptor.GetConverter(field.FieldType);
            if (!converter.IsValid(value))
                return;
            var result = converter.ConvertFrom(value);
            field.SetValue(null, result);
        }

        private static void ReadFile(Action<string> callback) {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs)) {
                while (!sr.EndOfStream) {
                    callback(sr.ReadLine());
                }
            }
        }

        public static void Set(string Key, object Value, Type storeType) {
            StoreType = storeType;
            path = GetPath();

            if (!File.Exists(path)) {
                var configs = GetProperties();
                WriteNewFile(configs);
            }

            Debug.WriteLine("SAVE " + Key + " (_" + Key + ") AS " + Value + " IN " + StoreType);

            FieldInfo property = StoreType.GetField("_" + Key, BindingFlags.NonPublic | BindingFlags.Static);
            if (property == null)
                return;

            Debug.WriteLine("SAVED");

            if (Value == null)
                Value = "";

            string text = "";
            GetFileProperties((k, v) => {
                if (k != Key) {
                    text += k + "=" + v + Environment.NewLine;
                    return;
                }
                text += k + "=" + Value + Environment.NewLine;
            });

            File.WriteAllText(path, text);
        }

        private static string GetPath() {
            FieldInfo field = StoreType.GetField("path", BindingFlags.Public | BindingFlags.Static);
            path = field.GetValue(null).ToString();
            return path;
        }
    }
}
