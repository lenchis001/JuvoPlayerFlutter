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

using System;
using System.Collections.Generic;
using System.Linq;
using static Configuration.ThroughputHistory;

namespace JuvoPlayer.DataProviders
{
    public class ThroughputHistory : IThroughputHistory
    {
        private readonly LinkedList<double> throughputs = new LinkedList<double>();

        public double GetAverageThroughput()
        {
            lock (throughputs)
            {
                var samplesCount = GetSamplesCount();
                return samplesCount > 0.0 ? throughputs.Take(samplesCount).Average() : 0.0;
            }
        }

        private int GetSamplesCount()
        {
            if (throughputs.Count < MinimumThroughputSampleAmount)
                return 0;

            var sampleSize = AverageThroughputSampleAmount;
            if (sampleSize >= throughputs.Count)
                return throughputs.Count;

            // if throughput samples vary a lot, average over a wider sample
            var first = throughputs.First;
            var second = first.Next;
            for (var i = 0; i < sampleSize - 1; ++i, first = second, second = second.Next)
            {
                var ratio = first.Value / second.Value;

                if (ratio >= ThroughputIncreaseScale || ratio <= 1 / ThroughputDecreaseScale)
                {
                    if (++sampleSize == throughputs.Count)
                        break;
                }
            }

            return sampleSize;
        }

        public void Push(int sizeInBytes, TimeSpan duration)
        {
            lock (throughputs)
            {
                // bits/ms = kbits/s
                var throughput = 8 * sizeInBytes / duration.TotalMilliseconds;
                throughputs.AddFirst(throughput * 1000); // we want throughputs in bps

                if (throughputs.Count > MaxMeasurementsToKeep)
                    throughputs.RemoveLast();
            }
        }

        public void Reset()
        {
            lock (throughputs)
            {
                throughputs.Clear();
            }
        }
    }
}
