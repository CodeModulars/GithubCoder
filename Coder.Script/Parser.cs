using Coder.Script.ScriptNodes;
using Coder.Script.ScriptStatements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 解析器
    /// </summary>
    public class Parser
    {
        // 字符串构建器
        private readonly StringBuilder _builder = new StringBuilder();
        // 语句集合
        private readonly List<IScriptStatement> _statements;

        // 首内容前空格数量
        private int _spaceCount;
        // 行数
        private int _line;
        // 位置
        private int _site;
        // 是否处于字符串
        private bool _isString;
        // 是否处于$模式
        private bool _isDollar;
        // 是否处于变量模式
        private bool _isVariable;
        // 当前行语句
        IScriptStatement? _statement;
        // 当前节点
        IScriptNode? _lastNode;

        /// <summary>
        /// 语句集合
        /// </summary>
        public List<IScriptStatement> Statements => _statements;

        /// <summary>
        /// 解析器
        /// </summary>
        /// <param name="script">脚本</param>
        public Parser(string script)
        {
            // 初始化
            _statements = new List<IScriptStatement>();
            _builder = new StringBuilder();
            _line = 1;
            _site = 0;
            _isVariable = false;
            _spaceCount = 0;
            _statement = null;
            _lastNode = null;
#if !DEBUG
            try
            {
#endif
            // 循环解析内容
            for (int i = 0; i < script.Length; i++)
            {
                _site++;
                char chr = script[i];
                switch (chr)
                {
                    // 处理空格
                    case ' ': ProcessSpace(chr); break;
                    case '\r': break;
                    // 处理换行
                    case '\n': ProcessWrap(chr); break;
                    // 处理"
                    case '"': ProcessDoubleQuotation(chr); break;
                    // 处理#
                    case '#': ProcessPound(chr); break;
                    // 处理$
                    case '$': ProcessDoller(chr); break;
                    // 处理@
                    case '@': ProcessAt(chr); break;
                    // 处理(
                    case '(': ProcessLeftBracket(chr); break;
                    // 处理)
                    case ')': ProcessRightBracket(chr); break;
                    // 默认处理
                    default: ProcessDefault(chr); break;
                }
            }
            // 末尾处理
            if (_builder.Length > 0)
            {
                if (_statement is null)
                {
                    _statement = new TextStatement();
                    _statements.Add(_statement);
                }
                // 指令结点空格为分隔符
                if (_statement is CommandStatement statement)
                {
                    ProcessCommandNodeEnd(statement, '\0');
                    return;
                }
                // 普通节点
                _statement.Nodes.Add(new TextNode(_builder.ToString()));
                _builder.Clear();
            }
#if !DEBUG
            }
            catch (ParseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"行{_line}位置{_site}解析发生异常：{ex.Message}", ex.InnerException);
            }
#endif
        }

        // 判断是否当前行无内容
        private bool IsNoneContent()
        {
            // 当前有语句，则不为无内容
            if (_statement != null) return false;
            // 空格数和内容长度相同，则代表当前行无内容
            return _spaceCount == _builder.Length;
        }

        // 处理@
        private void ProcessAt(char chr)
        {
            // 当前无内容，则进入命令语句行
            if (IsNoneContent())
            {
                if (_statement != null) throw new ParseException(_line, _site, chr);
                _statement = new CommandStatement();
                _statements.Add(_statement);
                _builder.Clear();
                return;
            };

            // 关闭$强制定义
            if (_isDollar) _isDollar = false;

            // 普通模式
            _builder.Append(chr);

        }

        // 处理(
        private void ProcessLeftBracket(char chr)
        {
            // 判断异常字符
            if (_isVariable) throw new ParseException(_line, _site, chr);

            // 普通文本
            if (IsNoneContent())
            {
                _statement = new TextStatement();
                _statements.Add(_statement);
                _spaceCount = 0;
            }

            switch (_statement)
            {
                // 文本类型
                case TextStatement _:
                    // 进入变量模式
                    if (_isDollar)
                    {
                        if (_builder.Length > 0)
                        {
                            _lastNode = new TextNode(_builder.ToString());
                            _statement.Nodes.Add(_lastNode);
                            _builder.Clear();
                        }
                        _isDollar = false;
                        _isVariable = true;
                        return;
                    }
                    _builder.Append(chr);
                    return;
                case NoteStatement _:
                    _builder.Append(chr);
                    return;
                // 指令类型
                case CommandStatement _:
                    // 判断是否在字符串中
                    if (_isString)
                    {
                        // 进入变量模式
                        if (_builder.Length > 0)
                        {
                            if (_lastNode is null) throw new ParseException(_line, _site, chr);
                            ((StringNode)_lastNode).Add(new TextNode(_builder.ToString()));
                            _builder.Clear();
                        }
                        _isDollar = false;
                        _isVariable = true;
                        return;
                    }
                    // 函数一定要出现在第3个节点
                    if (_statement.Nodes.Count != 2) throw new ParseException(_line, _site, chr);
                    // 前一个节点不能为空
                    if (_lastNode is null) throw new ParseException(_line, _site, chr);
                    if (_builder.Length <= 0) throw new ParseException(_line, _site, chr);
                    _lastNode = new FunctionNameNode(_builder.ToString());
                    _statement.Nodes.Add(_lastNode);
                    _builder.Clear();
                    return;
                default: throw new ParseException(_line, _site, chr);
            }
        }

        // 处理命令行 )
        private void ProcessCommandRightBracket(IScriptStatement statement, char chr)
        {
            // 必须不能没有节点
            if (statement.Nodes.Count < 1) throw new ParseException(_line, _site, chr);

            // 获取第一个节点
            if (!(statement.Nodes[0] is NameNode nameNode)) throw new ParseException(_line, _site, chr);

            switch (nameNode.Name)
            {
                case "call":
                    // 函数一定要出现在第3个节点之后
                    if (statement.Nodes.Count < 3) throw new ParseException(_line, _site, chr);
                    if (_lastNode is null) throw new ParseException(_line, _site, chr);
                    if (_builder.Length <= 0)
                    {
                        if (!(_lastNode is FunctionNameNode) && !(_lastNode is StringNode)) throw new ParseException(_line, _site, chr);
                    }
                    if (_builder.Length > 0)
                    {
                        if (!_isDollar) throw new ParseException(_line, _site, chr);
                        _lastNode = new FieldNode(_builder.ToString());
                        statement.Nodes.Add(_lastNode);
                        _isDollar = false;
                        _builder.Clear();
                    }
                    _lastNode = null;
                    break;
                default: throw new ParseException(_line, _site, chr);
            }
        }

        // 处理 )
        private void ProcessRightBracket(char chr)
        {
            // 普通文本
            if (IsNoneContent())
            {
                _statement = new TextStatement();
                _statements.Add(_statement);
                _spaceCount = 0;
            }

            switch (_statement)
            {
                // 文本类型
                case TextStatement _:
                    if (_isDollar)
                    {
                        _builder.Append('$');
                        _isDollar = false;
                    }
                    else
                    {
                        // 变量模式处理
                        if (_isVariable)
                        {
                            if (_builder.Length <= 0) throw new ParseException(_line, _site, chr);
                            _lastNode = new FieldNode(_builder.ToString());
                            _statement.Nodes.Add(_lastNode);
                            _isVariable = false;
                            _builder.Clear();
                            return;
                        }
                    }
                    _builder.Append(chr);
                    return;
                case NoteStatement _:
                    _builder.Append(chr);
                    return;
                // 指令类型
                case CommandStatement _:
                    // 变量模式处理
                    if (_isVariable)
                    {
                        if (_builder.Length <= 0) throw new ParseException(_line, _site, chr);
                        if (_lastNode is null) throw new ParseException(_line, _site, chr);
                        ((StringNode)_lastNode).Add(new FieldNode(_builder.ToString()));
                        _builder.Clear();
                        _isVariable = false;
                        _builder.Clear();
                        return;
                    }
                    ProcessCommandRightBracket(_statement, chr);
                    return;
                default: throw new ParseException(_line, _site, chr);
            }

        }

        // 处理命令结点结束
        private void ProcessCommandNodeEnd(CommandStatement statement, char chr)
        {
            // 过滤重复空格
            if (_builder.Length <= 0) return;

            // 判断是否为首结点
            if (!statement.Nodes.Any())
            {
                _lastNode = new NameNode(_builder.ToString());
                statement.Nodes.Add(_lastNode);
            }
            else if (_builder[0] == '$')
            {
                // 判断字符串是否结束
                if (_isString) throw new ParseException(_line, _site, chr);
                _lastNode = new FieldNode(_builder.ToString(1, _builder.Length - 1));
                statement.Nodes.Add(_lastNode);
            }
            //else if (_builder[0] == '"')
            //{
            //    // 判断字符串是否结束
            //    if (_isString) throw new ParseException(_line, _site, chr);
            //    _lastNode = new StringNode(_builder.ToString(1, _builder.Length - 2));
            //    statement.Nodes.Add(_lastNode);
            //}
            else
            {
                // 普通节点
                _lastNode = new TextNode(_builder.ToString());
                statement.Nodes.Add(_lastNode);
            }
            _builder.Clear();
        }

        // 处理$
        private void ProcessDoller(char chr)
        {
            // 处理首语句
            if (IsNoneContent())
            {
                _statement = new TextStatement();
                _statements.Add(_statement);
                _isDollar = true;
                _spaceCount = 0;
                return;
            }

            // 在文本中
            if (_statement is TextStatement)
            {
                if (_isDollar)
                {
                    _builder.Append(chr);
                    _isDollar = false;
                }
                else
                {
                    _isDollar = true;
                }
                return;
            }

            // 命令模式
            if (_statement is CommandStatement)
            {
                // 兼容字符串引用变量
                if (_isString)
                {
                    if (_isDollar)
                    {
                        _builder.Append(chr);
                        _isDollar = false;
                    }
                    else
                    {
                        _isDollar = true;
                    }
                    return;
                }
            }

            // 普通字符
            _builder.Append(chr);
        }

        // 处理#
        private void ProcessPound(char chr)
        {
            // 处理注释语句
            if (IsNoneContent())
            {
                _statement = new NoteStatement();
                _statements.Add(_statement);
                _spaceCount = 0;
                _builder.Clear();
                return;
            }

            // 关闭$强制定义
            if (_isDollar) _isDollar = false;

            // 普通字符
            _builder.Append(chr);
        }

        // 处理双引号 "
        private void ProcessDoubleQuotation(char chr)
        {
            // 当前无内容，则报错
            if (IsNoneContent()) throw new ParseException(_line, _site, chr);

            // 判断是否空语句
            if (_statement is null) throw new ParseException(_line, _site, chr);

            // 指令结点空格为分隔符
            if (_statement is CommandStatement)
            {
                if (_isDollar)
                {
                    _isDollar = false;
                    _builder.Append(chr);
                    return;
                }
                // 字符串结束
                if (_isString)
                {
                    if (_lastNode is null) throw new ParseException(_line, _site, chr);
                    var stringNode = (StringNode)_lastNode;
                    if (_builder.Length > 0)
                    {
                        stringNode.Add(new TextNode(_builder.ToString()));
                        _builder.Clear();
                        _isString = false;
                        return;
                    }
                }
                // 建立字符串
                _lastNode = new StringNode();
                _statement.Nodes.Add(_lastNode);
                _isString = true;
                return;
            }

            // 普通模式
            _builder.Append(chr);

        }

        // 处理空格
        private void ProcessSpace(char chr)
        {
            // 当前无内容，则空格数加1
            if (IsNoneContent())
            {
                _spaceCount++;
                _builder.Append(chr);
                return;
            }

            // 判断是否空语句
            if (_statement is null) throw new ParseException(_line, _site, chr);

            // 指令结点空格为分隔符
            if (_statement is CommandStatement statement)
            {
                ProcessCommandNodeEnd(statement, chr);
                return;
            }

            // 普通模式
            _builder.Append(chr);

        }

        // 移到下一行
        private void MoveNextLine()
        {
            // 更新行，同时初始化数据
            _line++;
            _site = 0;
            _spaceCount = 0;
            _statement = null;
            _lastNode = null;
        }

        // 处理换行
        private void ProcessWrap(char chr)
        {
            // 有内容则先处理行内容
            if (_builder.Length > 0)
            {
                // 当前无语句，则先添加语句
                if (_statement is null)
                {
                    _statement = new TextStatement();
                    _statements.Add(_statement);
                }
                // 指令结点空格为分隔符
                if (_statement is CommandStatement statement)
                {
                    ProcessCommandNodeEnd(statement, '\0');
                    MoveNextLine();
                    return;
                }
                // 在当前语句中添加文本结点
                _statement.Nodes.Add(new TextNode(_builder.ToString()));
                // 清理行内容
                _builder.Clear();
            }
            // 更新行，同时初始化数据
            MoveNextLine();
        }

        // 处理默认
        private void ProcessDefault(char chr)
        {
            // 当前无语句，则先添加语句
            if (_statement is null)
            {
                _statement = new TextStatement();
                _statements.Add(_statement);
            }

            // 兼容$符号
            if (_isDollar)
            {
                _builder.Append('$');
                _isDollar = false;
            }

            // 添加内容
            _builder.Append(chr);
        }
    }
}
