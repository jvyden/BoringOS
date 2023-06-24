using System.Diagnostics.CodeAnalysis;
using Cosmos.Core;
using Zarlo.Cosmos.Core;
using Zarlo.Cosmos.Threading.Core.Context;
using Zarlo.Cosmos.Threading.Core.Processing;
using ThreadState = Zarlo.Cosmos.Threading.Core.Context.ThreadState;

namespace BoringOS.Cosmos.ThreadingPlugs;

public static class BoringProcessScheduler
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public static void SwitchTask()
    {
        CPU.DisableInterrupts();
        ++ProcessorScheduler.interruptCount;

        if (ProcessContextManager.m_CurrentContext != null)
        {
            ProcessContext processContext1 = ProcessContextManager.m_ContextList;
            ProcessContext processContext2 = processContext1;
            for (; processContext1 != null; processContext1 = processContext1.next)
            {
                if (processContext1.state == ThreadState.DEAD)
                {
                    processContext2.next = processContext1.next;
                    break;
                }

                processContext2 = processContext1;
            }

            for (ProcessContext processContext3 = ProcessContextManager.m_ContextList;
                 processContext3 != null;
                 processContext3 = processContext3.next)
            {
                if (processContext3.state == ThreadState.WAITING_SLEEP)
                {
                    processContext3.arg -= 40;
                    if (processContext3.arg <= 0)
                        processContext3.state = ThreadState.ALIVE;
                }

                ++processContext3.age;
            }

            ProcessContextManager.m_CurrentContext.esp = ZINTs.mStackContext;
            do
            {
                ProcessContextManager.m_CurrentContext = ProcessContextManager.m_CurrentContext.next == null
                    ? ProcessContextManager.m_ContextList
                    : ProcessContextManager.m_CurrentContext.next;
            } while (ProcessContextManager.m_CurrentContext.state != 0);

            ProcessContextManager.m_CurrentContext.age = ProcessContextManager.m_CurrentContext.priority;
            ZINTs.mStackContext = ProcessContextManager.m_CurrentContext.esp;
        }

        Global.PIC.EoiMaster();
        Global.PIC.EoiSlave();

        CPU.EnableInterrupts();
    }
}