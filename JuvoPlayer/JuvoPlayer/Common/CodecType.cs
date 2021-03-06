/*!
 * https://github.com/SamsungDForum/JuvoPlayer
 * Copyright 2018, Samsung Electronics Co., Ltd
 * Licensed under the MIT license
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace JuvoPlayer.Common {
    public enum VideoCodec {
        H264 = 1,
        VC1 = 2,
        MPEG2 = 3,
        MPEG4 = 4,
        THEORA = 5,
        VP8 = 6,
        VP9 = 7,
        H263 = 8,
        WMV1 = 9,
        WMV2 = 10,
        WMV3 = 11,
        INDEO3 = 12,
        H265 = 13,
    }

    public enum AudioCodec {
        AAC = 1,
        MP3 = 2,
        PCM = 3,
        VORBIS = 4,
        FLAC = 5,
        AMR_NB = 6,
        AMR_WB = 7,
        PCM_MULAW = 8,
        GSM_MS = 9,
        PCM_S16BE = 10,
        PCM_S24BE = 11,
        OPUS = 12,
        EAC3 = 13,
        MP2 = 14,
        DTS = 15,
        AC3 = 16,
        WMAV1 = 17,
        WMAV2 = 18,
    }
}
