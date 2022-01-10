// Copyright 2022 Andreas Atteneder
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//


using System.IO;
using UnityEditor;
using UnityEngine;

namespace Unity.glTF.Validator.Editor {
    
    public static class MenuItems {
        
        static string SaveFolderPath {
            get {
                var saveFolderPath = EditorUserSettings.GetConfigValue("glTF.saveFilePath");
                if (string.IsNullOrEmpty(saveFolderPath)) {
                    saveFolderPath = Application.streamingAssetsPath;
                }
                return saveFolderPath;
            }
            set => EditorUserSettings.SetConfigValue("glTF.saveFilePath",value);
        }
        
        [MenuItem("Tools/glTF-Validator")]
        static void MenuGltfValidate() {
            var path = EditorUtility.OpenFilePanelWithFilters("Select glTF file to validate", SaveFolderPath, new []{"glTF files", "gltf,glb"} );
            if (!string.IsNullOrEmpty(path)) {
                SaveFolderPath = Directory.GetParent(path)?.FullName;
                var report = Validator.Validate(path);
                report.Log();
            }
        }
    }
}
