NVisitor
========

Lightweight Framework to develop modular extendable visitors

Introduction
------------

The visitor pattern is a paradox pattern: on the one hand it enables to *extend* a class 
family with any algorithm at any time: you just need to implement a new `Visitor`. On 
the other hand you cannot extend the family itself, let’s say the `Node` family. 
Following code illustrates the issue:

```

```

You can declare new `Node` types, but you cannot add a special handling

