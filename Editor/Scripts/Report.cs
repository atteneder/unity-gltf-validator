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


using System;
using System.Linq;
using UnityEngine;

namespace Unity.glTF.Validator {

    public enum MessageCode {
        Unknown,
        // ReSharper disable InconsistentNaming
        ACCESSOR_ELEMENT_OUT_OF_MAX_BOUND,
        ACCESSOR_MAX_MISMATCH,
        ACCESSOR_MIN_MISMATCH,
        NODE_EMPTY,
        UNUSED_OBJECT,
        // ReSharper restore InconsistentNaming
    }
    
    [Serializable]
    public class Report {
        public string uri;
        public string mimeType;
        public string validatorVersion;

        public Issue issues;
        public Info info;

        public static readonly MessageCode[] defaultIgnored = new[] {
            MessageCode.UNUSED_OBJECT
        };

        public void Log(MessageCode[] ignoredCodes = null) {
            if (issues != null) {
                foreach (var message in issues.messages) {
                    if(ignoredCodes!=null && ignoredCodes.Contains(message.codeEnum) ) continue;
                    message.Log();
                }
            } else {
                Debug.Log("glTF validation passed");
            }
        }
    }

    [Serializable]
    public class Issue {
        public int numErrors;
        public int numWarnings;
        public int numInfos;
        public int numHints;
        public Message[] messages;
        public bool truncated;
    }
    
    [Serializable]
    public class Message {
        public string code;
        public string message;
        public int severity;
        public string pointer;

        public MessageCode codeEnum => Enum.TryParse<MessageCode>(code, out var value) ? value : MessageCode.Unknown;

        public void Log() {
            if (severity < 1) {
                Debug.LogError($"{message} ({code}, {pointer})");
            } else if (severity < 2) {
                Debug.LogWarning($"{message} ({code}, {pointer})");
            } else {
                Debug.Log($"{message} ({code}, {pointer})");
            }
        }

        public override string ToString() {
            return $"{code}: {message} ({pointer})";
        }
    }
    
    [Serializable]
    public class Info {
        public string version;
        public string generator;
        public string[] extensionsUsed;
        public string[] extensionsRequired;
            
        public int animationCount;
        public int materialCount;
        public int drawCallCount;
        public int totalVertexCount;
        public int totalTriangleCount;
        public int maxUVs;
        public int maxInfluences;
        public int maxAttributes;
            
        public bool hasMorphTargets;
        public bool hasSkins;
        public bool hasTextures;
        public bool hasDefaultScene;
    }

    [Serializable]
    public class Resource {
        public string pointer;
        public string mimeTyp;
        public string storage;
        public string uri;
        public int byteLength;
        public Image image;
    }

    [Serializable]
    public class Image {
        public int width;
        public int height;
        public string format;
        public string primaries;
        public string transfer;
        public int bits;
    }
}
