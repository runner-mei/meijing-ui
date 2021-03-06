/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*
 *
 *  Marshaler code for OpenWire format for XATransactionId
 *
 *  NOTE!: This file is auto generated - do not modify!
 *         if you need to make a change, please see the Java Classes
 *         in the nms-activemq-openwire-generator module
 *
 */

using System;
using System.IO;

using Apache.NMS.ActiveMQ.Commands;

namespace Apache.NMS.ActiveMQ.OpenWire.V6
{
    /// <summary>
    ///  Marshalling code for Open Wire Format for XATransactionId
    /// </summary>
    class XATransactionIdMarshaller : TransactionIdMarshaller
    {
        /// <summery>
        ///  Creates an instance of the Object that this marshaller handles.
        /// </summery>
        public override DataStructure CreateObject() 
        {
            return new XATransactionId();
        }

        /// <summery>
        ///  Returns the type code for the Object that this Marshaller handles..
        /// </summery>
        public override byte GetDataStructureType() 
        {
            return XATransactionId.ID_XATRANSACTIONID;
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void TightUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn, BooleanStream bs) 
        {
            base.TightUnmarshal(wireFormat, o, dataIn, bs);

            XATransactionId info = (XATransactionId)o;
            info.FormatId = dataIn.ReadInt32();
            info.GlobalTransactionId = ReadBytes(dataIn, bs.ReadBoolean());
            info.BranchQualifier = ReadBytes(dataIn, bs.ReadBoolean());
        }

        //
        // Write the booleans that this object uses to a BooleanStream
        //
        public override int TightMarshal1(OpenWireFormat wireFormat, Object o, BooleanStream bs)
        {
            XATransactionId info = (XATransactionId)o;

            int rc = base.TightMarshal1(wireFormat, o, bs);
            bs.WriteBoolean(info.GlobalTransactionId!=null);
            rc += info.GlobalTransactionId==null ? 0 : info.GlobalTransactionId.Length+4;
            bs.WriteBoolean(info.BranchQualifier!=null);
            rc += info.BranchQualifier==null ? 0 : info.BranchQualifier.Length+4;

            return rc + 4;
        }

        // 
        // Write a object instance to data output stream
        //
        public override void TightMarshal2(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut, BooleanStream bs)
        {
            base.TightMarshal2(wireFormat, o, dataOut, bs);

            XATransactionId info = (XATransactionId)o;
            dataOut.Write(info.FormatId);
            if(bs.ReadBoolean()) {
                dataOut.Write(info.GlobalTransactionId.Length);
                dataOut.Write(info.GlobalTransactionId);
            }
            if(bs.ReadBoolean()) {
                dataOut.Write(info.BranchQualifier.Length);
                dataOut.Write(info.BranchQualifier);
            }
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void LooseUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn) 
        {
            base.LooseUnmarshal(wireFormat, o, dataIn);

            XATransactionId info = (XATransactionId)o;
            info.FormatId = dataIn.ReadInt32();
            info.GlobalTransactionId = ReadBytes(dataIn, dataIn.ReadBoolean());
            info.BranchQualifier = ReadBytes(dataIn, dataIn.ReadBoolean());
        }

        // 
        // Write a object instance to data output stream
        //
        public override void LooseMarshal(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut)
        {

            XATransactionId info = (XATransactionId)o;

            base.LooseMarshal(wireFormat, o, dataOut);
            dataOut.Write(info.FormatId);
            dataOut.Write(info.GlobalTransactionId!=null);
            if(info.GlobalTransactionId!=null) {
               dataOut.Write(info.GlobalTransactionId.Length);
               dataOut.Write(info.GlobalTransactionId);
            }
            dataOut.Write(info.BranchQualifier!=null);
            if(info.BranchQualifier!=null) {
               dataOut.Write(info.BranchQualifier.Length);
               dataOut.Write(info.BranchQualifier);
            }
        }
    }
}
