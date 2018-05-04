﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class AwaitHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new AsyncAwaitHighlighter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestExample2_2()
        {
            await TestAsync(
@"using System;
using System.Threading.Tasks;

class AsyncExample
{
    async Task<int> AsyncMethod()
    {
        int hours = 24;
        return hours;
    }

    [|async|] Task UseAsync()
    {
        Func<Task<int>> lambda = async () =>
        {
            return await AsyncMethod();
        };
        int result = {|Cursor:[|await|]|} AsyncMethod();
        Task<int> resultTask = AsyncMethod();
        result = [|await|] resultTask;
        result = [|await|] lambda();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestExample2_3()
        {
            await TestAsync(
@"using System;
using System.Threading.Tasks;

class AsyncExample
{
    async Task<int> AsyncMethod()
    {
        int hours = 24;
        return hours;
    }

    [|async|] Task UseAsync()
    {
        Func<Task<int>> lambda = async () =>
        {
            return await AsyncMethod();
        };
        int result = [|await|] AsyncMethod();
        Task<int> resultTask = AsyncMethod();
        result = {|Cursor:[|await|]|} resultTask;
        result = [|await|] lambda();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestExample2_4()
        {
            await TestAsync(
@"using System;
using System.Threading.Tasks;

class AsyncExample
{
    async Task<int> AsyncMethod()
    {
        int hours = 24;
        return hours;
    }

    [|async|] Task UseAsync()
    {
        Func<Task<int>> lambda = async () =>
        {
            return await AsyncMethod();
        };
        int result = [|await|] AsyncMethod();
        Task<int> resultTask = AsyncMethod();
        result = [|await|] resultTask;
        result = {|Cursor:[|await|]|} lambda();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestExample3_2()
        {
            await TestAsync(
@"using System;
using System.Threading.Tasks;

class AsyncExample
{
    async Task<int> AsyncMethod()
    {
        int hours = 24;
        return hours;
    }

    async Task UseAsync()
    {
        Func<Task<int>> lambda = [|async|] () =>
        {
            return {|Cursor:[|await|]|} AsyncMethod();
        };
        int result = await AsyncMethod();
        Task<int> resultTask = AsyncMethod();
        result = await resultTask;
        result = await lambda();
    }
}");
        }

        [WorkItem(573625, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/573625")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestNestedAwaits1()
        {
            await TestAsync(
@"using System;
using System.Threading.Tasks;

class AsyncExample
{
    async Task<Task<int>> AsyncMethod()
    {
        return NewMethod();
    }

    private static Task<int> NewMethod()
    {
        int hours = 24;
        return hours;
    }

    async Task UseAsync()
    {
        Func<Task<int>> lambda = [|async|] () =>
        {
            return {|Cursor:[|await await|]|} AsyncMethod();
        };
        int result = await await AsyncMethod();
        Task<Task<int>> resultTask = AsyncMethod();
        result = await await resultTask;
        result = await lambda();
    }
}");
        }

        [WorkItem(573625, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/573625")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestNestedAwaits2()
        {
            await TestAsync(
@"using System;
using System.Threading.Tasks;

class AsyncExample
{
    async Task<Task<int>> AsyncMethod()
    {
        return NewMethod();
    }

    private static Task<int> NewMethod()
    {
        int hours = 24;
        return hours;
    }

    [|async|] Task UseAsync()
    {
        Func<Task<int>> lambda = async () =>
        {
            return await await AsyncMethod();
        };
        int result = {|Cursor:[|await await|]|} AsyncMethod();
        Task<Task<int>> resultTask = AsyncMethod();
        result = [|await await|] resultTask;
        result = [|await|] lambda();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestUsingAwait_OnAsync()
        {
            await TestAsync(
@"using System.Threading.Tasks;

class C
{
    {|Cursor:[|async|]|} Task M()
    {
        using [|await|] (var x = new object());
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestUsingAwait_OnAwait()
        {
            await TestAsync(
@"using System.Threading.Tasks;

class C
{
    [|async|] Task M()
    {
        using {|Cursor:[|await|]|} (var x = new object());
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestForEachAwait_OnAsync()
        {
            await TestAsync(
@"using System.Threading.Tasks;

class C
{
    {|Cursor:[|async|]|} Task M()
    {
        foreach [|await|] (var n in new int[] { });
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestForEachAwait_OnAwait()
        {
            await TestAsync(
@"using System.Threading.Tasks;

class C
{
    [|async|] Task M()
    {
        foreach {|Cursor:[|await|]|} (var n in new int[] { });
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestForEachVariableAwait_OnAsync()
        {
            await TestAsync(
@"using System.Threading.Tasks;

class C
{
    {|Cursor:[|async|]|} Task M()
    {
        foreach [|await|] (var (a, b) in new (int, int)[] { });
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
        public async Task TestForEachVariableAwait_OnAwait()
        {
            await TestAsync(
@"using System.Threading.Tasks;

class C
{
    [|async|] Task M()
    {
        foreach {|Cursor:[|await|]|} (var (a, b) in new (int, int)[] { });
    }
}");
        }
    }
}
