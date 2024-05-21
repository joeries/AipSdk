/*
 * Copyright 2017 Baidu, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Baidu.Aip.Speech
{
    /// <summary>
    ///     长文本语音合成任务创建结果
    /// </summary>
    public class LongTtsCreateResponse
    {
        public long log_id { get; set; }
        public string task_id { get; set; }
        public string task_status { get; set; }
        public int status { get; set; }
        public string error { get; set; }
        public string message { get; set; }

        public bool Success
        {
            get { return status == 0; }
        }
    }

    /// <summary>
    ///     长文本语音合成任务查询结果
    /// </summary>
    public class LongTtsQueryResponse
    {
        public long log_id { get; set; }
        public int error_code { get; set; }
        public string error_msg { get; set; }
        public List<string> error_info { get; set; }
        public List<TaskInfo> tasks_info {  get; set; }

        public bool Success
        {
            get { return error_code == 0; }
        }
    }

    public class TaskInfo
    {
        public string task_id { get; set; }
        public string task_status { get; set; }
        public TaskResult task_result { get; set; }
    }

    public class TaskResult
    {
        public int err_no { get; set; }
        public string err_msg { get; set; }
        public string sn { get; set; }
        public string speech_url { get; set; }
        public SpeechTimestamp speech_timestamp { get; set; }
    }

    public class SpeechTimestamp
    {
        public List<SentenceInfo> sentences { get; set; }
        public List<CharacterInfo> characters { get; set; }
    }

    public class SentenceInfo
    {
        public int paragraph_index { get; set; }
        public string sentence_texts { get; set; }
        public int begin_time { get; set; }
        public int end_time { get; set; }
    }

    public class CharacterInfo
    {
        public string character_text { get; set; }
        public int begin_time { get; set; }
        public int end_time { get; set; }
    }

    /// <summary>
    ///     长文本语音合成相关接口
    /// </summary>
    public class LongTts : Base
    {
        private const string UrlLongTts = "https://aip.baidubce.com/rpc/2.0/tts/v1/";

        public LongTts(string apiKey, string secretKey) : base(apiKey, secretKey)
        {
        }

        protected AipHttpRequest DefaultRequest(string uri)
        {
            return new AipHttpRequest(uri)
            {
                Method = "POST",
                BodyType = AipHttpRequest.BodyFormat.Json,
                ContentEncoding = Encoding.UTF8
            };
        }

        /// <summary>
        ///     长文本语音合成任务创建
        /// </summary>
        /// <param name="text">需要合成的内容</param>
        /// <param name="options">format, voice, speed, pitch, volume, enable_subtitle, break</param>
        /// <returns></returns>
        public LongTtsCreateResponse SynthesisCreate(List<string> texts, Dictionary<string, object> options = null)
        {
            PreAction();
            CheckNotNull(texts, "text");
            CheckListCountNotZero<string>(texts, "text");
            var req = DefaultRequest($"{UrlLongTts}create?access_token={Token}");

            if (options != null)
                foreach (var pair in options)
                    req.Bodys[pair.Key] = pair.Value;

            if (!req.Bodys.ContainsKey("lang"))
                req.Bodys["lang"] = "zh";

            req.Bodys["text"] = texts;
            return PostAction<LongTtsCreateResponse>(req);
        }

        /// <summary>
        ///     长文本语音合成任务查询
        /// </summary>
        /// <param name="task_ids">需要查询的任务ID</param>
        /// <returns></returns>
        public LongTtsQueryResponse SynthesisQuery(List<string> task_ids)
        {
            PreAction();
            CheckNotNull(task_ids, "task_ids");
            CheckListCountNotZero<string>(task_ids, "task_ids");
            var req = DefaultRequest($"{UrlLongTts}query?access_token={Token}");    
            req.Bodys["task_ids"] = task_ids;
            return PostAction<LongTtsQueryResponse>(req);
        }

        protected new T PostAction<T>(AipHttpRequest aipReq)
        {
            var ret = default(T);
            var response = SendRequetRaw(aipReq);

            var respStr = Utils.StreamToString(response.GetResponseStream(), Encoding.UTF8);
            try
            {
                ret = JsonConvert.DeserializeObject<T>(respStr);              
            }
            catch (Exception e)
            {
                // 返回非json
                throw new AipException(e.Message + ": " + respStr);
            }
            return ret;
        }
    }
}