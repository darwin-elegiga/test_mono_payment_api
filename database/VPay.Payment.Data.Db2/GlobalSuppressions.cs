// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Models map to DB2 objects, which contain underscores",
    Scope = "namespaceanddescendants",
    Target = "~N:VPay.Payment.Data.Db2.Models")]
