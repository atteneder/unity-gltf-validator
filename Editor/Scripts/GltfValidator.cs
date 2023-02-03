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

using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Unity.glTF.Validator
{
    public static class Validator
    {
        const string k_CommonValidatorPath = "Packages/com.unity.formats.gltf.validator/Apps~/gltf_validator-2.0.0-dev.3.8-";

        public static Report Validate(string path)
        {
            var validatorPath = GetValidatorPath();

            var processInfo = new ProcessStartInfo
            {
                FileName = validatorPath,
                Arguments = $"-o \"{path}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var process = Process.Start(processInfo);

            if (process != null)
            {
                var json = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                return JsonUtility.FromJson<Report>(json);
            }

            return null;
        }

        static string GetValidatorPath()
        {
            var relativePath = k_CommonValidatorPath + GetPlatformSubPath();
            var fullPath = Path.GetFullPath(relativePath);
            return fullPath;
        }

        static string GetPlatformSubPath()
        {
#if UNITY_EDITOR_WIN
            return "win64/gltf_validator.exe";
#elif UNITY_EDITOR_OSX
            return "macos64/gltf_validator";
#elif UNITY_EDITOR_LINUX
            return "linux64/gltf_validator";
#endif
        }
    }
}
