// <copyright file="ISerializable.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

namespace Datadog.Sketches.Serialization
{
    /// <summary>
    /// Object serializable to protobuf using the dedicated serializer
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// Gets the needed size for serialization
        /// </summary>
        int ComputeSerializedSize();

        /// <summary>
        /// Serialize to protobuf using the given serializer
        /// </summary>
        /// <param name="serializer">Serializer used to manually encode the object to protobuf</param>
        void Serialize(Serializer serializer);
    }
}
