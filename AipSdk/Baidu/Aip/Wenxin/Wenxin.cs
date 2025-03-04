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

using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Baidu.Aip.Wenxin
{
    /// <summary>
    /// 基础版AI作画提交响应
    /// </summary>
    public class BasicTextToImageResponse
    {
        /// <summary>
        /// 错误号
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string error_msg { get; set; }
        /// <summary>
        /// 错误详情
        /// </summary>
        public List<BasicTextToImageResponseError> error_detail { get; set; }
        /// <summary>
        /// 请求唯一标识码
        /// </summary>
        public long log_id { get; set; }
        /// <summary>
        /// 结果对象，返回 task id。任务完成后，作为获取图片的依据
        /// </summary>
        public BasicTextToImageResponseData data { get; set; }
    }

    /// <summary>
    /// 基础版AI作画提交错误
    /// </summary>
    public class BasicTextToImageResponseError
    {
        /// <summary>
        /// 不合规项描述信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 送检文本命中词库的关键词
        /// </summary>
        public List<string> words { get; set; }
    }

    /// <summary>
    /// 基础版AI作画提交结果
    /// </summary>
    public class BasicTextToImageResponseData
    {
        /// <summary>
        /// 图片生成任务id，作为查询接口的入参
        /// </summary>
        public long taskId { get; set; }
        /// <summary>
        /// 生成图片任务string类型 id，与“taskId”参数输出相同，该 id 可用于查询任务状态
        /// </summary>
        public string primaryTaskId { get; set; }
    }

    /// <summary>
    /// 基础版AI作画查询响应
    /// </summary>
    public class BasicGetImgResponse
    {
        /// <summary>
        /// 错误号
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string error_msg { get; set; }
        /// <summary>
        /// 请求唯一标识码
        /// </summary>
        public long log_id { get; set; }
        /// <summary>
        /// task_id对应请求的任务状态和生成结果
        /// </summary>
        public BasicGetImgResponseData data { get; set; }
    }

    /// <summary>
    /// 基础版AI作画查询结果
    /// </summary>
    public class BasicGetImgResponseData
    {
        /// <summary>
        /// 请求内容中的图片风格
        /// </summary>
        public string style { get; set; }

        /// <summary>
        /// 对应任务的id
        /// </summary>
        public long taskId { get; set; }

        /// <summary>
        ///  生成结果数组（目前默认生成1张图）
        /// </summary>
        public List<BasicGetImgResponseDataImage> imgUrls { get; set; }

        /// <summary>
        /// 请求内容中的文本
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 0或1。"1"表示已生成完成，"0"表示任务排队中或正在处理。
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public string createTime { get; set; }

        /// <summary>
        /// 生成结果地址
        /// </summary>
        public string img { get; set; }

        /// <summary>
        /// 预计等待时间（仅供参考）
        /// </summary>
        public string waiting { get; set; }
    }

    /// <summary>
    /// 图片详情
    /// </summary>
    public class BasicGetImgResponseDataImage
    {
        /// <summary>
        /// 生成结果地址（有效期30天），在使用图片链接时需附带上authorization的鉴权信息，请使用完整的图片的链接
        /// </summary>
        public string image { get; set; }

        /// <summary>
        /// 图片审核结果：pass
        /// </summary>
        public string img_approve_conclusion { get; set; }
    }

    /// <summary>
    /// AI作画
    /// </summary>
    public class Wenxin : AipServiceBase
    {
        private const string BASIC_TEXTTOIMAGE = "https://aip.baidubce.com/rpc/2.0/wenxin/v1/basic/textToImage";

        private const string BASIC_GETIMG = "https://aip.baidubce.com/rpc/2.0/wenxin/v1/basic/getImg";

        public Wenxin(string appId, string apiKey, string secretKey) : base(appId, apiKey, secretKey)
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
        /// 基础版AI作画提交
        /// </summary>
        /// <param name="text">输入内容，仅支持中文、英文、日常标点符号（~！@#$%&*()-+[];:'',./）。不支持特殊符号，长度不超过190个字</param>
        /// <param name="resolution">图片分辨率，可支持512*512、640*360、360*640、1024*1024、720*1280、1280*720</param>
        /// <param name="style">目前支持风格有：二次元、写实风格、古风、赛博朋克、水彩画、油画、卡通画</param>
        /// <param name="num">图片生成数量，支持1-6张</param>
        /// <param name="text_content">1~50个字符，支持英文、数字及常用特殊字符。若不传该参数则默认为Text-to-Image-内容ID-AI，示例：Text-to-Image-131870381_0_finaI.png-Al；若传该参数，则相应水印内容自动添加至Text-to-Image后方，示例：Text-to-Image-ABCD-131870381_0_finaI.png-Al</param>
        /// <returns></returns>
        public BasicTextToImageResponse BasicTextToImage(string text, string resolution = "512*512", string style = "写实风格", int num = 1, string text_content = "TCY")
        {
            var aipReq = DefaultRequest($"{BASIC_TEXTTOIMAGE}");

            aipReq.Bodys["text"] = text;
            aipReq.Bodys["resolution"] = resolution;
            aipReq.Bodys["style"] = style;
            aipReq.Bodys["num"] = num;
            aipReq.Bodys["text_content"] = text_content;
            PreAction();

            return PostAction<BasicTextToImageResponse>(aipReq);
        }

        /// <summary>
        /// 基础版AI作画查询
        /// </summary>
        /// <param name="taskId">从提交请求的提交接口的返回值中获取</param>
        /// <returns></returns>
        public BasicGetImgResponse BasicGetImg(long taskId)
        {
            var aipReq = DefaultRequest($"{BASIC_GETIMG}");

            aipReq.Bodys["taskId"] = taskId;
            PreAction();

            return PostAction<BasicGetImgResponse>(aipReq);
        }
    }
}