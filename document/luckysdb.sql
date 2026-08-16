--
-- PostgreSQL database dump
--

\restrict oe0ZtKDoRbUJ1dlF6lWmfZdUmRNHwh29IrfAobYVpUSubg73UtDRArgi79rMy1a

-- Dumped from database version 18.2
-- Dumped by pg_dump version 18.2

-- Started on 2026-08-16 22:20:05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 40971)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO xiaoxiao;

--
-- TOC entry 220 (class 1259 OID 40976)
-- Name: prtcl; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.prtcl (
    id integer NOT NULL,
    name character varying(40) NOT NULL
);


ALTER TABLE public.prtcl OWNER TO xiaoxiao;

--
-- TOC entry 5019 (class 0 OID 0)
-- Dependencies: 220
-- Name: TABLE prtcl; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON TABLE public.prtcl IS '协议公用表';


--
-- TOC entry 5020 (class 0 OID 0)
-- Dependencies: 220
-- Name: COLUMN prtcl.id; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.prtcl.id IS '主键';


--
-- TOC entry 5021 (class 0 OID 0)
-- Dependencies: 220
-- Name: COLUMN prtcl.name; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.prtcl.name IS '名称';


--
-- TOC entry 221 (class 1259 OID 40981)
-- Name: prtcl_grpc; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.prtcl_grpc (
    id integer NOT NULL,
    name character varying(20) NOT NULL
);


ALTER TABLE public.prtcl_grpc OWNER TO xiaoxiao;

--
-- TOC entry 5022 (class 0 OID 0)
-- Dependencies: 221
-- Name: TABLE prtcl_grpc; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON TABLE public.prtcl_grpc IS 'grpc协议';


--
-- TOC entry 5023 (class 0 OID 0)
-- Dependencies: 221
-- Name: COLUMN prtcl_grpc.id; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.prtcl_grpc.id IS 'id';


--
-- TOC entry 5024 (class 0 OID 0)
-- Dependencies: 221
-- Name: COLUMN prtcl_grpc.name; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.prtcl_grpc.name IS '名称';


--
-- TOC entry 222 (class 1259 OID 40986)
-- Name: sys_config; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_config (
    id integer NOT NULL,
    cfg_type character varying(40),
    name character varying(40) NOT NULL,
    value character varying(40),
    code character varying(40),
    sort integer NOT NULL,
    status integer NOT NULL,
    is_system boolean NOT NULL,
    is_del boolean NOT NULL,
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    update_time timestamp without time zone,
    update_uid bigint,
    del_time timestamp without time zone,
    del_uid bigint,
    type_name character(60)
);


ALTER TABLE public.sys_config OWNER TO xiaoxiao;

--
-- TOC entry 235 (class 1259 OID 57353)
-- Name: sys_config_id_seq; Type: SEQUENCE; Schema: public; Owner: xiaoxiao
--

ALTER TABLE public.sys_config ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.sys_config_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 223 (class 1259 OID 40997)
-- Name: sys_log; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_log (
    id bigint NOT NULL,
    req_url character varying(100),
    req_params character varying(400),
    req_ip character varying(20),
    status integer NOT NULL,
    err_msg character varying(400),
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    req_type character(10),
    exec_time numeric(10,2)
);


ALTER TABLE public.sys_log OWNER TO xiaoxiao;

--
-- TOC entry 5025 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.id; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.id IS '主键Id';


--
-- TOC entry 5026 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.req_url; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.req_url IS '接口地址';


--
-- TOC entry 5027 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.req_params; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.req_params IS '请求参数';


--
-- TOC entry 5028 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.req_ip; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.req_ip IS '请求IP';


--
-- TOC entry 5029 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.status; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.status IS '执行状态';


--
-- TOC entry 5030 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.err_msg; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.err_msg IS '错误信息';


--
-- TOC entry 5031 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.create_time; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.create_time IS '创建时间';


--
-- TOC entry 5032 (class 0 OID 0)
-- Dependencies: 223
-- Name: COLUMN sys_log.create_uid; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_log.create_uid IS '创建人';


--
-- TOC entry 224 (class 1259 OID 41007)
-- Name: sys_log_id_seq; Type: SEQUENCE; Schema: public; Owner: xiaoxiao
--

ALTER TABLE public.sys_log ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 225 (class 1259 OID 41008)
-- Name: sys_menu; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_menu (
    id integer NOT NULL,
    name character varying(40) NOT NULL,
    code character varying(10),
    icon character varying(20),
    icon_size character varying(10),
    path character varying(60),
    parent_id integer,
    sort integer,
    menu_type integer NOT NULL,
    is_hidden boolean NOT NULL,
    status integer NOT NULL,
    is_del boolean NOT NULL,
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    update_time timestamp without time zone,
    update_uid bigint,
    del_time timestamp without time zone,
    del_uid bigint
);


ALTER TABLE public.sys_menu OWNER TO xiaoxiao;

--
-- TOC entry 226 (class 1259 OID 41019)
-- Name: sys_org; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_org (
    id integer NOT NULL,
    name character varying(60) NOT NULL,
    org_type integer NOT NULL,
    code character varying(10),
    parent_id integer NOT NULL,
    leader_id bigint NOT NULL,
    phone character varying(20),
    remark character varying(200),
    is_del boolean NOT NULL,
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    update_time timestamp without time zone,
    update_uid bigint,
    del_time timestamp without time zone,
    del_uid bigint
);


ALTER TABLE public.sys_org OWNER TO xiaoxiao;

--
-- TOC entry 227 (class 1259 OID 41030)
-- Name: sys_role; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_role (
    id integer NOT NULL,
    name character varying(40) NOT NULL,
    word character varying(20),
    sort integer NOT NULL,
    status integer NOT NULL,
    remark character varying(200),
    is_del boolean NOT NULL,
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    update_time timestamp without time zone,
    update_uid bigint,
    del_time timestamp without time zone,
    del_uid bigint,
    role_type smallint
);


ALTER TABLE public.sys_role OWNER TO xiaoxiao;

--
-- TOC entry 5033 (class 0 OID 0)
-- Dependencies: 227
-- Name: COLUMN sys_role.role_type; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_role.role_type IS '角色类型';


--
-- TOC entry 228 (class 1259 OID 41040)
-- Name: sys_role_menu; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_role_menu (
    role_id integer NOT NULL,
    menu_id integer NOT NULL,
    id integer NOT NULL
);


ALTER TABLE public.sys_role_menu OWNER TO xiaoxiao;

--
-- TOC entry 233 (class 1259 OID 49161)
-- Name: sys_role_menu_id_seq; Type: SEQUENCE; Schema: public; Owner: xiaoxiao
--

CREATE SEQUENCE public.sys_role_menu_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sys_role_menu_id_seq OWNER TO xiaoxiao;

--
-- TOC entry 5034 (class 0 OID 0)
-- Dependencies: 233
-- Name: sys_role_menu_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: xiaoxiao
--

ALTER SEQUENCE public.sys_role_menu_id_seq OWNED BY public.sys_role_menu.id;


--
-- TOC entry 234 (class 1259 OID 49170)
-- Name: sys_role_menu_id_seq1; Type: SEQUENCE; Schema: public; Owner: xiaoxiao
--

ALTER TABLE public.sys_role_menu ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.sys_role_menu_id_seq1
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
    CYCLE
);


--
-- TOC entry 237 (class 1259 OID 57371)
-- Name: sys_tsk; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_tsk (
    id integer NOT NULL,
    name character(60) NOT NULL,
    code character(40) NOT NULL,
    status integer DEFAULT 0,
    cron character(60),
    param_model character(600),
    remark text,
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    update_time timestamp without time zone,
    update_uid bigint,
    del_time timestamp without time zone,
    del_uid bigint,
    is_del boolean DEFAULT false NOT NULL
);


ALTER TABLE public.sys_tsk OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 57370)
-- Name: sys_tsk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_tsk ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.sys_tsk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 238 (class 1259 OID 57384)
-- Name: sys_tsk_record; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_tsk_record (
    id bigint NOT NULL,
    tsk_id integer NOT NULL,
    tsk_param text,
    tsk_msg text,
    status integer,
    start_time timestamp without time zone,
    end_time timestamp without time zone,
    create_time timestamp without time zone
);


ALTER TABLE public.sys_tsk_record OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 41045)
-- Name: sys_user; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_user (
    id bigint NOT NULL,
    account character varying(40) NOT NULL,
    password character varying(40) NOT NULL,
    realname character varying(30),
    email character varying(40),
    avatar character varying(120),
    phone character varying(20),
    status integer NOT NULL,
    is_del boolean NOT NULL,
    create_time timestamp without time zone NOT NULL,
    create_uid bigint NOT NULL,
    update_time timestamp without time zone,
    update_uid bigint,
    del_time timestamp without time zone,
    del_uid bigint,
    sex integer
);


ALTER TABLE public.sys_user OWNER TO xiaoxiao;

--
-- TOC entry 5035 (class 0 OID 0)
-- Dependencies: 229
-- Name: COLUMN sys_user.sex; Type: COMMENT; Schema: public; Owner: xiaoxiao
--

COMMENT ON COLUMN public.sys_user.sex IS '性别';


--
-- TOC entry 230 (class 1259 OID 41056)
-- Name: sys_user_menu; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_user_menu (
    user_id bigint NOT NULL,
    menu_id integer NOT NULL
);


ALTER TABLE public.sys_user_menu OWNER TO xiaoxiao;

--
-- TOC entry 231 (class 1259 OID 41061)
-- Name: sys_user_org; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_user_org (
    user_id bigint NOT NULL,
    org_id integer NOT NULL
);


ALTER TABLE public.sys_user_org OWNER TO xiaoxiao;

--
-- TOC entry 232 (class 1259 OID 41066)
-- Name: sys_user_role; Type: TABLE; Schema: public; Owner: xiaoxiao
--

CREATE TABLE public.sys_user_role (
    user_id bigint NOT NULL,
    role_id integer NOT NULL,
    id bigint
);


ALTER TABLE public.sys_user_role OWNER TO xiaoxiao;

--
-- TOC entry 4994 (class 0 OID 40971)
-- Dependencies: 219
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
\.


--
-- TOC entry 4995 (class 0 OID 40976)
-- Dependencies: 220
-- Data for Name: prtcl; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.prtcl (id, name) FROM stdin;
1	xjw
\.


--
-- TOC entry 4996 (class 0 OID 40981)
-- Dependencies: 221
-- Data for Name: prtcl_grpc; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.prtcl_grpc (id, name) FROM stdin;
\.


--
-- TOC entry 4997 (class 0 OID 40986)
-- Dependencies: 222
-- Data for Name: sys_config; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_config (id, cfg_type, name, value, code, sort, status, is_system, is_del, create_time, create_uid, update_time, update_uid, del_time, del_uid, type_name) FROM stdin;
1		性别类型	SexType	sexType	0	1	t	f	2026-08-02 23:34:23.118607	1	\N	\N	\N	\N	                                                            
2	SexType	男	male	male	1	1	f	f	2026-08-02 23:43:38.988905	1	2026-08-02 23:53:37.686123	1	\N	\N	性别类型                                                        
3	SexType	女	female	female	2	1	f	f	2026-08-02 23:43:59.252213	1	2026-08-02 23:53:40.850999	1	\N	\N	性别类型                                                        
5		AI大模型类型	AIType	AIType	0	1	t	f	2026-08-02 23:56:40.528997	1	\N	\N	\N	\N	                                                            
6	AIType	千问	Qwen	Qwen	0	1	t	f	2026-08-02 23:57:20.504746	1	\N	\N	\N	\N	AI大模型类型                                                     
7	AIType	深度求索	DeepSeek	DeepSeek	0	1	t	f	2026-08-02 23:58:11.234943	1	\N	\N	\N	\N	AI大模型类型                                                     
8	AIType	豆包	Doubao	Doubao	0	1	t	f	2026-08-02 23:58:26.616096	1	\N	\N	\N	\N	AI大模型类型                                                     
9	AIType	测试001	test001	test001	0	1	t	t	2026-08-02 23:59:51.895221	1	\N	\N	2026-08-03 00:01:06.288737	1	AI大模型类型                                                     
10	AIType	测试001	test001	test001	0	1	t	t	2026-08-02 23:59:53.191822	1	\N	\N	2026-08-03 00:01:08.882208	1	AI大模型类型                                                     
11	AIType	智谱	Zhipu	Zhipu	0	1	t	t	2026-08-03 18:06:42.395376	1	\N	\N	2026-08-03 18:06:56.36698	1	AI大模型类型                                                     
12	AIType	小米	xmi	xmi	0	1	f	f	2026-08-03 18:11:18.128225	1	2026-08-03 19:52:05.674321	1	\N	\N	AI大模型类型                                                     
13	SexType	未知	Unknow	Unknow	0	1	t	f	2026-08-03 19:52:33.420185	1	\N	\N	\N	\N	性别类型                                                        
14		系统常量类型	ConstType	ConstType	0	1	f	f	2026-08-10 16:07:17.556116	1	2026-08-16 13:14:17.189975	1	\N	\N	                                                            
15	ConstType	测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称	testConstValue	testConstValue	0	1	t	f	2026-08-16 21:36:00.076483	1	\N	\N	\N	\N	系统常量类型                                                      
\.


--
-- TOC entry 4998 (class 0 OID 40997)
-- Dependencies: 223
-- Data for Name: sys_log; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_log (id, req_url, req_params, req_ip, status, err_msg, create_time, create_uid, req_type, exec_time) FROM stdin;
3437498783764549	/api/sys/SysUser/Permissions	{}	::1	1	\N	2026-08-05 16:18:20.505885	1	GET       	371.78
3437498783764550	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	1	\N	2026-08-05 16:18:20.505915	1	GET       	215.91
3437498861645893	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	1	\N	2026-08-05 16:18:39.519818	1	GET       	12.36
3437498863759429	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "2"\r\n}	::1	1	\N	2026-08-05 16:18:40.03566	1	GET       	2.43
3437498864410693	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "2"\r\n}	::1	1	\N	2026-08-05 16:18:40.194755	1	GET       	2.82
3437498865205317	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "2"\r\n}	::1	1	\N	2026-08-05 16:18:40.388035	1	GET       	2.33
3437498936397893	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "SysUser",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "2"\r\n}	::1	1	\N	2026-08-05 16:18:57.769877	1	GET       	24.07
3437498957344837	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:19:02.883162	1	GET       	1.99
3437498990866501	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "7"\r\n}	::1	1	\N	2026-08-05 16:19:11.06795	1	GET       	1.99
3437499206672453	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "8"\r\n}	::1	1	\N	2026-08-05 16:20:03.754984	1	GET       	8.57
3437499753803845	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "9"\r\n}	::1	1	\N	2026-08-05 16:22:17.331691	1	GET       	2.63
3437500355063877	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "10"\r\n}	::1	1	\N	2026-08-05 16:24:44.12384	1	GET       	137687.70
3437500576682053	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "10"\r\n}	::1	1	\N	2026-08-05 16:25:38.229902	1	GET       	22510.79
3437500850942021	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "12"\r\n}	::1	1	\N	2026-08-05 16:26:45.187338	1	GET       	14629.63
3437501017276485	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "/SysUser",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "13"\r\n}	::1	1	\N	2026-08-05 16:27:25.796187	1	GET       	23410.18
3437501051789381	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "/SysUser",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:27:34.222	1	GET       	5781.13
3437501061976133	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "/SysUser",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:27:36.709092	1	GET       	3.34
3437501077336133	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "/SysUser",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:27:40.459917	1	GET       	3.66
3437501118554181	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "/SysUser",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:27:50.522604	1	GET       	2.15
3437501148221509	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:27:57.765798	1	GET       	2.73
3437501636517957	/api/sys/SysRole/pages	{\r\n  "name": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	1	\N	2026-08-05 16:29:56.978828	1	GET       	196.56
3437501636517958	/api/sys/SysMenu/getMenuSelTree	{}	::1	1	\N	2026-08-05 16:29:56.978856	1	GET       	162.98
3437501640585285	/api/sys/SysRole/pages	{\r\n  "name": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "4"\r\n}	::1	1	\N	2026-08-05 16:29:57.971796	1	GET       	20.72
3437501673328709	/api/sys/SysRole/roleSels	{}	::1	1	\N	2026-08-05 16:30:05.965635	1	GET       	17.52
3437501673406533	/api/sys/SysOrg/treeSels	{}	::1	1	\N	2026-08-05 16:30:05.984699	1	GET       	2.89
3437501673918533	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	1	\N	2026-08-05 16:30:06.109216	1	GET       	162.34
3437501698314309	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "4"\r\n}	::1	1	\N	2026-08-05 16:30:12.065693	1	GET       	3.19
3437501902225477	/api/sys/SysUser	{\r\n  "input": {\r\n    "Id": null,\r\n    "Account": "tst007",\r\n    "Sex": 1,\r\n    "RoleId": "6,3",\r\n    "Name": "零零7",\r\n    "OrgId": null,\r\n    "Status": 0,\r\n    "Phone": "15131136537"\r\n  }\r\n}	::1	1	\N	2026-08-05 16:31:01.848571	1	POST      	75.26
3437501902286917	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "4"\r\n}	::1	1	\N	2026-08-05 16:31:01.863795	1	GET       	3.17
3437502012010565	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	1	\N	2026-08-05 16:31:28.651045	1	GET       	48.37
3437502019747909	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "29"\r\n}	::1	1	\N	2026-08-05 16:31:30.540883	1	GET       	2.22
3437502020554821	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "30"\r\n}	::1	1	\N	2026-08-05 16:31:30.737029	1	GET       	4.09
3437502079868997	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "POST",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "30"\r\n}	::1	1	\N	2026-08-05 16:31:45.218771	1	GET       	24.58
3437502092034117	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "1"\r\n}	::1	1	\N	2026-08-05 16:31:48.188877	1	GET       	3.96
3437502156726341	/api/sys/SysConfig/pages	{\r\n  "txt": "",\r\n  "cfgType": "",\r\n  "total": "0",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	1	\N	2026-08-05 16:32:03.982454	1	GET       	24.05
3437502156746821	/api/sys/SysConfig/list	{}	::1	1	\N	2026-08-05 16:32:03.987334	1	GET       	30.27
3437781644406855	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-06 11:29:18.279101	0	GET       	91.72
3437781644402757	/api/sys/SysOrg/treeSels	{}	::1	0	UnAuth	2026-08-06 11:29:18.279083	0	GET       	91.47
3437781644406853	/api/sys/SysRole/roleSels	{}	::1	0	UnAuth	2026-08-06 11:29:18.279113	0	GET       	110.27
3437781644406854	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 11:29:18.279145	0	GET       	110.27
3437806381654085	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-06 13:09:57.646297	0	LOGIN     	149.20
3437934018048069	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 20,\r\n    "ParentId": 18,\r\n    "MenuType": 3,\r\n    "Name": "办公智能体",\r\n    "Code": "20",\r\n    "Icon": "icon-bg",\r\n    "IconSize": 21,\r\n    "Url": "/sys/setting",\r\n    "Sort": 0,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-06 21:49:18.875709	1	PUT       	210.66
3437941237989445	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:41.556375	0	GET       	0.14
3437941237989446	/api/sys/SysMenu/getMenuTree	{\r\n  "name": ""\r\n}	::1	0	UnAuth	2026-08-06 22:18:41.55641	0	GET       	0.34
3437941237989447	/api/sys/SysMenu/getMenuSelTree	{}	::1	0	UnAuth	2026-08-06 22:18:41.556441	0	GET       	0.03
3437941239267397	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:41.868032	0	GET       	0.07
3437941242105925	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:42.561354	0	GET       	0.05
3437941242110021	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-06 22:18:42.562352	0	GET       	0.05
3437941242519621	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:42.662929	0	GET       	0.05
3437941243314245	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:42.856037	0	GET       	0.05
3437941243314246	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-06 22:18:42.856128	0	GET       	0.04
3437941243904069	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:43.000589	0	GET       	0.26
3437941243908165	/api/sys/SysConfig/list	{}	::1	0	UnAuth	2026-08-06 22:18:43.001317	0	GET       	0.06
3437941243908166	/api/sys/SysConfig/pages	{\r\n  "txt": "",\r\n  "cfgType": "",\r\n  "total": "0",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	UnAuth	2026-08-06 22:18:43.001904	0	GET       	0.08
3437941244563525	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-06 22:18:43.161944	0	GET       	0.05
3437941244563526	/api/sys/SysMenu/getMenuSelTree	{}	::1	0	UnAuth	2026-08-06 22:18:43.161945	0	GET       	0.05
3437941244567621	/api/sys/SysRole/pages	{\r\n  "name": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-06 22:18:43.16256	0	GET       	0.05
3437941269209157	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-06 22:18:49.178927	0	LOGIN     	50.74
3437941348556869	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHg=",\r\n    "Passwd": "384ad0e93259c24d048d0d5107dd8bed"\r\n  }\r\n}	::1	1	\N	2026-08-06 22:19:08.550036	0	LOGIN     	2.81
3437952139759685	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "ea077eb8d5122bd130f1acdd8b81b194"\r\n  }\r\n}	::1	1	\N	2026-08-06 23:03:03.121043	0	LOGIN     	179.83
3437952144379973	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-06 23:03:04.249383	0	LOGIN     	1.19
3438115045810245	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-07 10:05:55.107264	0	GET       	111.70
3438115045814341	/api/sys/SysConfig/list	{}	::1	0	UnAuth	2026-08-07 10:05:55.107315	0	GET       	111.75
3438115045814342	/api/sys/SysConfig/pages	{\r\n  "txt": "",\r\n  "cfgType": "",\r\n  "total": "0",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	UnAuth	2026-08-07 10:05:55.107328	0	GET       	86.01
3438115061809221	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-07 10:05:59.012522	0	GET       	0.17
3438115061858373	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-07 10:05:59.024377	0	GET       	0.13
3438115066433605	/api/sys/SysConfig/list	{}	::1	0	UnAuth	2026-08-07 10:06:00.141351	0	GET       	0.10
3438115066433606	/api/sys/SysConfig/pages	{\r\n  "txt": "",\r\n  "cfgType": "",\r\n  "total": "0",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	UnAuth	2026-08-07 10:06:00.141509	0	GET       	0.05
3438115066445893	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-07 10:06:00.144388	0	GET       	0.08
3438115070365765	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-07 10:06:01.10134	0	GET       	0.07
3438115070365766	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-07 10:06:01.101426	0	GET       	0.04
3438115073163333	/api/sys/SysConfig/list	{}	::1	0	UnAuth	2026-08-07 10:06:01.784965	0	GET       	0.05
3438115073163334	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-07 10:06:01.784965	0	GET       	0.05
3438115073167429	/api/sys/SysConfig/pages	{\r\n  "txt": "",\r\n  "cfgType": "",\r\n  "total": "0",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	UnAuth	2026-08-07 10:06:01.785091	0	GET       	0.03
3438115105189957	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "ea077eb8d5122bd130f1acdd8b81b194"\r\n  }\r\n}	::1	1	\N	2026-08-07 10:06:09.603561	0	LOGIN     	72.54
3438115110727749	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-07 10:06:10.955814	0	LOGIN     	11.10
3438120512196678	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-07 10:28:09.673761	0	GET       	0.52
3438120512196677	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-07 10:28:09.67376	0	GET       	0.64
3438120549683269	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-07 10:28:18.825371	0	LOGIN     	13.84
3438519178862661	/api/sys/SysLog/pages	{\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "reqType": "",\r\n  "status": "-1",\r\n  "reqUrl": "",\r\n  "reqIp": "",\r\n  "beginTime": "",\r\n  "endTime": "",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-08 13:30:20.403155	0	GET       	88.35
3438519178866757	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-08 13:30:20.403187	0	GET       	115.76
3438519215640645	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-08 13:30:29.381736	0	LOGIN     	91.28
3438543770607685	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-08 15:10:24.246744	0	GET       	0.12
3438543773040709	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-08 15:10:24.840578	0	GET       	0.05
3438543773040710	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-08 15:10:24.840585	0	GET       	0.02
3438543773040711	/api/sys/SysRole/roleSels	{}	::1	0	UnAuth	2026-08-08 15:10:24.840655	0	GET       	0.02
3438543773065285	/api/sys/SysOrg/treeSels	{}	::1	0	UnAuth	2026-08-08 15:10:24.84623	0	GET       	0.04
3438543773651013	/api/sys/SysMenu/getMenuTree	{\r\n  "name": ""\r\n}	::1	0	UnAuth	2026-08-08 15:10:24.989285	0	GET       	0.03
3438543773651014	/api/sys/SysMenu/getMenuSelTree	{}	::1	0	UnAuth	2026-08-08 15:10:24.989286	0	GET       	0.04
3438543773651015	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-08 15:10:24.989314	0	GET       	0.02
3438543774523461	/api/sys/SysMenu/getMenuSelTree	{}	::1	0	UnAuth	2026-08-08 15:10:25.202238	0	GET       	0.05
3438543774523462	/api/sys/SysRole/pages	{\r\n  "name": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-08 15:10:25.202658	0	GET       	0.07
3438543774523463	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-08 15:10:25.202852	0	GET       	0.03
3438543800868933	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-08 15:10:31.634689	0	LOGIN     	2.35
3438544079163461	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 0,\r\n    "ParentId": 18,\r\n    "MenuType": 3,\r\n    "Name": "具身智能体",\r\n    "Code": "21",\r\n    "Icon": "icon-robot",\r\n    "IconSize": 20,\r\n    "Url": "/sys/setting",\r\n    "Sort": 0,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-08 15:11:39.577831	1	POST      	98.72
3439262570844229	/api/sys/SysRole/roleSels	{}	::1	0	UnAuth	2026-08-10 15:55:12.585283	0	GET       	149.06
3439262570844230	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-10 15:55:12.585283	0	GET       	128.49
3439262570844231	/api/sys/SysOrg/treeSels	{}	::1	0	UnAuth	2026-08-10 15:55:12.585287	0	GET       	128.23
3439262570844232	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-10 15:55:12.585297	0	GET       	149.04
3439262623273029	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "ea077eb8d5122bd130f1acdd8b81b194"\r\n  }\r\n}	::1	1	\N	2026-08-10 15:55:25.385865	0	LOGIN     	64.53
3439262627823685	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-10 15:55:26.496639	0	LOGIN     	11.14
3439264404009029	/api/sys/SysConfig	{\r\n  "input": {\r\n    "Id": -1,\r\n    "Name": "系统常量类型",\r\n    "Value": "SystemConstType",\r\n    "CfgType": "",\r\n    "TypeName": "",\r\n    "Sort": 0,\r\n    "Status": 1,\r\n    "Code": "SystemConstType",\r\n    "IsSystem": null\r\n  }\r\n}	::1	0	An error occurred while saving the entity changes. See the inner exception for details.	2026-08-10 16:02:40.135044	1	POST      	14177.31
3439265216794693	/api/sys/SysConfig	{\r\n  "input": {\r\n    "Id": -1,\r\n    "Name": "系统常量类型",\r\n    "Value": "SysConstType",\r\n    "CfgType": "",\r\n    "TypeName": "",\r\n    "Sort": 0,\r\n    "Status": 1,\r\n    "Code": "SysConstType",\r\n    "IsSystem": null\r\n  }\r\n}	::1	0	An error occurred while saving the entity changes. See the inner exception for details.	2026-08-10 16:05:58.569291	1	POST      	32249.13
3439265266212933	/api/sys/SysConfig	{\r\n  "input": {\r\n    "Id": -1,\r\n    "Name": "系统常量类型",\r\n    "Value": "SysConstType",\r\n    "CfgType": "",\r\n    "TypeName": "",\r\n    "Sort": 0,\r\n    "Status": 1,\r\n    "Code": "SysConstType",\r\n    "IsSystem": null\r\n  }\r\n}	::1	0	An error occurred while saving the entity changes. See the inner exception for details.	2026-08-10 16:06:10.634847	1	POST      	5060.89
3439265540415557	/api/sys/SysConfig	{\r\n  "input": {\r\n    "Id": -1,\r\n    "Name": "系统常量类型",\r\n    "Value": "SysConstType",\r\n    "CfgType": "",\r\n    "TypeName": "",\r\n    "Sort": 0,\r\n    "Status": 1,\r\n    "Code": "SysConstType",\r\n    "IsSystem": null\r\n  }\r\n}	::1	1	\N	2026-08-10 16:07:17.578365	1	POST      	22.40
3439266910806085	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-10 16:12:52.146969	0	LOGIN     	50.18
3439990844592197	/api/sys/SysOrg/treeSels	{}	::1	0	UnAuth	2026-08-12 17:18:33.793549	0	GET       	97.03
3439990844592198	/api/sys/SysUser/pages	{\r\n  "txt": "",\r\n  "orgId": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-12 17:18:33.793553	0	GET       	97.80
3439990844592199	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-12 17:18:33.793588	0	GET       	128.51
3439990844592200	/api/sys/SysRole/roleSels	{}	::1	0	UnAuth	2026-08-12 17:18:33.793589	0	GET       	128.45
3439990947528773	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-12 17:18:58.92479	0	LOGIN     	71.63
3439991308767301	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 0,\r\n    "ParentId": 0,\r\n    "MenuType": 1,\r\n    "Name": "定时任务",\r\n    "Code": "22",\r\n    "Icon": "icon-time-task",\r\n    "IconSize": 21,\r\n    "Url": "/",\r\n    "Sort": 0,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:20:27.11769	1	POST      	91.96
3439991674437701	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 0,\r\n    "ParentId": 22,\r\n    "MenuType": 3,\r\n    "Name": "任务管理",\r\n    "Code": "23",\r\n    "Icon": "icon-order-manage",\r\n    "IconSize": 21,\r\n    "Url": "/sys/tsk",\r\n    "Sort": 1,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:21:56.39227	1	POST      	4.59
3439991877296197	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 0,\r\n    "ParentId": 22,\r\n    "MenuType": 3,\r\n    "Name": "任务执行记录",\r\n    "Code": "24",\r\n    "Icon": "icon-instruction",\r\n    "IconSize": 21,\r\n    "Url": "/sys/tskRecord",\r\n    "Sort": 2,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:22:45.918031	1	POST      	30.17
3439991993585733	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 22,\r\n    "ParentId": 0,\r\n    "MenuType": 1,\r\n    "Name": "定时任务",\r\n    "Code": "22",\r\n    "Icon": "icon-time-task",\r\n    "IconSize": 23,\r\n    "Url": "/",\r\n    "Sort": 4,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:23:14.309244	1	PUT       	3.04
3439991943209029	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 22,\r\n    "ParentId": 0,\r\n    "MenuType": 1,\r\n    "Name": "定时任务",\r\n    "Code": "22",\r\n    "Icon": "icon-time-task",\r\n    "IconSize": 21,\r\n    "Url": "/",\r\n    "Sort": 4,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:23:02.01064	1	PUT       	25.04
3439992137863237	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 22,\r\n    "ParentId": 0,\r\n    "MenuType": 1,\r\n    "Name": "定时任务",\r\n    "Code": "22",\r\n    "Icon": "icon-reloadtime",\r\n    "IconSize": 23,\r\n    "Url": "/",\r\n    "Sort": 4,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:23:49.533664	1	PUT       	3.46
3439992181678149	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 22,\r\n    "ParentId": 0,\r\n    "MenuType": 1,\r\n    "Name": "定时任务",\r\n    "Code": "22",\r\n    "Icon": "icon-reloadtime",\r\n    "IconSize": 22,\r\n    "Url": "/",\r\n    "Sort": 4,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 17:24:00.230226	1	PUT       	3.27
3440065883566149	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 22,\r\n    "ParentId": 0,\r\n    "MenuType": 1,\r\n    "Name": "任务管理",\r\n    "Code": "22",\r\n    "Icon": "icon-reloadtime",\r\n    "IconSize": 22,\r\n    "Url": "/",\r\n    "Sort": 4,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 22:23:53.856167	1	PUT       	286.55
3440065936433221	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 23,\r\n    "ParentId": 22,\r\n    "MenuType": 3,\r\n    "Name": "定时任务",\r\n    "Code": "23",\r\n    "Icon": "icon-order-manage",\r\n    "IconSize": 21,\r\n    "Url": "/sys/tsk",\r\n    "Sort": 1,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 22:24:06.762625	1	PUT       	2.55
3440066013102149	/api/sys/SysMenu	{\r\n  "input": {\r\n    "Id": 24,\r\n    "ParentId": 22,\r\n    "MenuType": 3,\r\n    "Name": "任务执行记录",\r\n    "Code": "24",\r\n    "Icon": "icon-instruction",\r\n    "IconSize": 21,\r\n    "Url": "/sys/tskRecord",\r\n    "Sort": 2,\r\n    "Status": 1\r\n  }\r\n}	::1	1	\N	2026-08-12 22:24:25.480206	1	PUT       	3.08
3440075243958341	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	42P01: relation "sys_tsk" does not exist\r\n\r\nPOSITION: 28	2026-08-12 23:01:59.107296	1	GET       	7829.04
3440075324981317	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	42P01: relation "sys_tsk" does not exist\r\n\r\nPOSITION: 28	2026-08-12 23:02:18.888325	1	GET       	17313.32
3440079050018885	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	42703: column s.is_del does not exist\r\n\r\nPOSITION: 53	2026-08-12 23:17:28.321287	1	GET       	8112.57
3440079106867269	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	42703: column s.is_del does not exist\r\n\r\nPOSITION: 53	2026-08-12 23:17:42.200649	1	GET       	9716.89
3440079683039301	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	42804: argument of NOT must be type boolean, not type integer\r\n\r\nPOSITION: 53	2026-08-12 23:20:02.867471	1	GET       	44607.76
3440599881629766	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-14 10:36:44.476355	0	GET       	90.49
3440599881629765	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-14 10:36:44.476324	0	GET       	126.95
3440606403014725	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-14 11:03:16.611807	0	LOGIN     	140.24
3440618823966789	/api/sys/SysTsk	{\r\n  "input": {\r\n    "Id": 0,\r\n    "Name": "定时获取设备通行记录",\r\n    "Code": "Device_Pass_Record",\r\n    "Cron": "\\"* * * * *\\"",\r\n    "ParamModel": "{\\n      \\"deviceId\\":\\"abc666\\",\\n       \\"passPic\\": \\"http://123.png\\"\\n}",\r\n    "Status": 0,\r\n    "Remark": "哈哈哈"\r\n  }\r\n}	::1	1	\N	2026-08-14 11:53:49.070439	1	POST      	8022.44
3440619593064517	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	Reading as 'System.DateTime' is not supported for fields having DataTypeName 'time without time zone'	2026-08-14 11:56:56.838585	1	GET       	25107.31
3440619601035333	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20"\r\n}	::1	0	Reading as 'System.DateTime' is not supported for fields having DataTypeName 'time without time zone'	2026-08-14 11:56:58.784075	1	GET       	839.47
3440622660337733	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	Reading as 'System.DateTime' is not supported for fields having DataTypeName 'time without time zone'	2026-08-14 12:09:25.684178	1	GET       	733642.05
3440623630450757	/api/sys/SysTsk	{\r\n  "input": {\r\n    "Id": 0,\r\n    "Name": "定时获取设备通行记录",\r\n    "Code": "Device_Pass_Record",\r\n    "Cron": "\\"* * * * *\\"",\r\n    "ParamModel": "{\\"deviceId\\":\\"xyz\\",\\"passPic\\":\\"http://\\"}",\r\n    "Status": 0,\r\n    "Remark": "lalala"\r\n  }\r\n}	::1	1	\N	2026-08-14 12:13:22.528329	1	POST      	2146.05
3440697592000581	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-14 17:14:19.547514	0	GET       	4.81
3440697592000582	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-14 17:14:19.547646	0	GET       	4.75
3440697635397701	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-14 17:14:30.142608	0	LOGIN     	58.20
3440702547669061	/api/sys/SysTsk/status/0	{\r\n  "id": 0\r\n}	::1	1	\N	2026-08-14 17:34:29.427697	1	PUT       	1918.47
3440702718439493	/api/sys/SysTsk/status/0	{\r\n  "id": 0\r\n}	::1	1	\N	2026-08-14 17:35:11.119957	1	PUT       	9120.68
3440702854471749	/api/sys/SysTsk/status/2	{\r\n  "id": 2\r\n}	::1	1	\N	2026-08-14 17:35:44.330776	1	PUT       	3387.95
3440703073984581	/api/sys/SysTsk/status/2	{\r\n  "id": 2\r\n}	::1	1	\N	2026-08-14 17:36:37.922607	1	PUT       	4.01
3440703089082437	/api/sys/SysTsk/status/2	{\r\n  "id": 2\r\n}	::1	1	\N	2026-08-14 17:36:41.608594	1	PUT       	3.27
3440999796887622	/api/sys/SysLog/pages	{\r\n  "tskId": "4",\r\n  "status": "-1",\r\n  "startTime": "",\r\n  "endTime": "",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-15 13:44:00.037589	0	GET       	90.83
3440999796887621	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-15 13:44:00.037586	0	GET       	117.74
3440999900053573	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-15 13:44:25.224349	0	LOGIN     	93.99
3441298533634118	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-16 09:59:33.813769	0	GET       	77.14
3441298533634117	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-16 09:59:33.813766	0	GET       	96.87
3441304354177093	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-16 10:23:14.844891	0	LOGIN     	300.19
3441436343160901	/api/sys/SysTsk/pages	{\r\n  "txt": "",\r\n  "status": "-1",\r\n  "pageIndex": "1",\r\n  "pageSize": "20",\r\n  "total": "0"\r\n}	::1	0	UnAuth	2026-08-16 19:20:18.717949	0	GET       	84.28
3441436343160902	/api/sys/SysUser/Permissions	{}	::1	0	UnAuth	2026-08-16 19:20:18.717982	0	GET       	112.10
3441436381741125	/api/sys/SysUser/loginHdl	{\r\n  "req": {\r\n    "Account": "eHhpYW8=",\r\n    "Passwd": "845d5f1153c27beed29f479640445148"\r\n  }\r\n}	::1	1	\N	2026-08-16 19:20:28.136222	0	LOGIN     	92.03
3441469787131973	/api/sys/SysConfig	{\r\n  "input": {\r\n    "Id": -1,\r\n    "Name": "测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称",\r\n    "Value": "testConstValue",\r\n    "CfgType": "ConstType",\r\n    "TypeName": "系统常量类型",\r\n    "Sort": 0,\r\n    "Status": 1,\r\n    "Code": "testConstValue",\r\n    "IsSystem": null\r\n  }\r\n}	::1	1	\N	2026-08-16 21:36:23.749584	1	POST      	24107.68
3441470253805637	/api/sys/SysConfig	{\r\n  "input": {\r\n    "Id": -1,\r\n    "Name": "测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称测试常量名称",\r\n    "Value": "tstConstVal",\r\n    "CfgType": "ConstType",\r\n    "TypeName": "系统常量类型",\r\n    "Sort": 0,\r\n    "Status": 1,\r\n    "Code": "tstConstVal",\r\n    "IsSystem": null\r\n  }\r\n}	::1	0	An error occurred while saving the entity changes. See the inner exception for details.	2026-08-16 21:38:17.683677	1	POST      	12649.79
\.


--
-- TOC entry 5000 (class 0 OID 41008)
-- Dependencies: 225
-- Data for Name: sys_menu; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_menu (id, name, code, icon, icon_size, path, parent_id, sort, menu_type, is_hidden, status, is_del, create_time, create_uid, update_time, update_uid, del_time, del_uid) FROM stdin;
1	系统管理	01	icon-system-locked	21	/	0	1	1	f	1	f	2026-05-14 15:36:27	1	\N	\N	\N	\N
3	菜单管理	03	icon-a-grid	20	/sys/menu	1	2	3	f	1	f	2026-05-14 16:19:18	1	\N	\N	\N	\N
5	字典设置	05	icon-DropboxOutlined	20	/sys/loginlog	4	1	3	f	1	f	2026-05-27 17:56:11	1	\N	\N	\N	\N
7	权限管理	07	icon-yonghuguanli	20	/sys/permission	1	3	3	f	1	f	2026-05-28 11:01:01	1	\N	\N	\N	\N
2	用户管理	02	icon-jiaose	18	/sys/user	1	1	3	f	1	f	2026-05-14 16:18:19	1	\N	\N	\N	\N
8	系统设置	08	icon-order-manage	21	/sys/setting	1	5	3	f	1	f	2026-05-28 16:16:08	1	\N	\N	\N	\N
9	系统日志	09	icon-assignment	19	/sys/loginlog	1	6	3	f	1	f	2026-05-28 16:18:19	1	\N	\N	\N	\N
4	设置管理	04	icon-settings	21	/sys/setting	1	7	2	f	1	f	2026-05-27 17:54:51	1	\N	\N	\N	\N
11	参数设置	11	icon-settings	21	/	4	3	3	f	1	f	2026-07-19 11:13:00.236984	1	\N	\N	\N	\N
14	AI解答	14	icon-chat	21	/ai/chat	12	1	3	f	1	f	2026-07-19 22:00:35.789226	1	2026-07-19 22:02:55.899533	1	\N	\N
10	缓存设置	10	icon-data	20	/	4	0	3	f	2	f	2026-07-15 23:06:28.779825	1	2026-07-19 22:04:09.247642	1	\N	\N
16	AI测试	16	icon-tst	20	/	12	0	3	f	1	f	2026-07-21 12:30:47.902604	1	\N	\N	\N	\N
12	AI管理	12	icon-AIchuangzuo	22	/	0	2	1	f	1	f	2026-07-19 21:55:46.339966	1	2026-07-23 18:42:37.603642	1	\N	\N
6	常量设置	06	icon-a-036_fuwuqi	23	/sys/org	4	2	3	f	1	f	2026-05-27 15:57:27	1	2026-07-23 18:43:50.771323	1	\N	\N
19	家电智能体	19	icon-electric	20	/agent/electric	18	0	3	f	1	f	2026-07-29 17:18:39.630654	1	\N	\N	\N	\N
15	智能体管理	15	icon-a-grid	19	/ai/agent	12	3	3	f	1	f	2026-07-19 22:01:27.93404	1	2026-08-03 11:47:47.865585	1	\N	\N
17	ceshi	17	icon-ceshi	21	/	12	0	3	f	0	f	2026-07-21 13:40:43.111058	1	2026-08-03 11:47:52.711294	1	\N	\N
13	大模型管理	13	icon-settings	20	/sys/setting	12	0	3	f	1	f	2026-07-19 21:57:59.301305	1	2026-08-04 13:23:35.423805	1	\N	\N
18	智能体管理	18	icon-app	20	/	0	3	1	f	1	f	2026-07-29 17:16:10.124549	1	2026-08-04 16:47:54.968621	1	\N	\N
20	办公智能体	20	icon-bg	21	/sys/setting	18	0	3	f	1	f	2026-08-03 11:47:31.506679	1	2026-08-06 21:49:18.109312	1	\N	\N
21	具身智能体	21	icon-robot	20	/sys/setting	18	0	3	f	1	f	2026-08-08 15:11:39.498326	1	\N	\N	\N	\N
22	任务管理	22	icon-reloadtime	22	/	0	4	1	f	1	f	2026-08-12 17:20:27.047396	1	2026-08-12 22:23:53.018839	1	\N	\N
23	定时任务	23	icon-order-manage	21	/sys/tsk	22	1	3	f	1	f	2026-08-12 17:21:56.389148	1	2026-08-12 22:24:06.761046	1	\N	\N
24	任务执行记录	24	icon-instruction	21	/sys/tskRecord	22	2	3	f	1	f	2026-08-12 17:22:45.889534	1	2026-08-12 22:24:25.478107	1	\N	\N
\.


--
-- TOC entry 5001 (class 0 OID 41019)
-- Dependencies: 226
-- Data for Name: sys_org; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_org (id, name, org_type, code, parent_id, leader_id, phone, remark, is_del, create_time, create_uid, update_time, update_uid, del_time, del_uid) FROM stdin;
\.


--
-- TOC entry 5002 (class 0 OID 41030)
-- Dependencies: 227
-- Data for Name: sys_role; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_role (id, name, word, sort, status, remark, is_del, create_time, create_uid, update_time, update_uid, del_time, del_uid, role_type) FROM stdin;
1	超级管理员	SuperAdmin	1	1	superAdmin	f	2026-01-09 16:16:16	1	\N	\N	\N	\N	3
2	管理员	Admin	2	1	admin	f	2026-01-09 16:18:19	1	\N	\N	\N	\N	2
4	测试角色	Test	1	1	test	t	2026-05-14 17:47:49	0	2026-07-16 13:19:27.681952	1	2026-07-16 13:20:17.71176	1	2
3	普通用户	Common	3	1	common	f	2026-05-14 17:47:49	1	2026-07-19 10:50:55.601968	1	\N	\N	1
5	测试角色	testRole	4	1	tst	t	2026-07-16 16:03:00.978152	1	2026-07-19 10:51:00.716246	1	2026-07-19 10:52:35.8959	1	2
6	测试角色	tstRole	5	1	测试备注	f	2026-07-19 10:53:11.865988	1	2026-07-20 11:35:13.586158	1	\N	\N	2
\.


--
-- TOC entry 5003 (class 0 OID 41040)
-- Dependencies: 228
-- Data for Name: sys_role_menu; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_role_menu (role_id, menu_id, id) FROM stdin;
5	5	1
5	6	2
5	8	3
3	5	17
3	6	18
3	8	19
3	9	20
3	10	21
2	5	29
2	6	30
2	8	31
2	9	32
2	10	33
2	13	34
2	14	35
2	15	36
6	9	37
\.


--
-- TOC entry 5012 (class 0 OID 57371)
-- Dependencies: 237
-- Data for Name: sys_tsk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sys_tsk (id, name, code, status, cron, param_model, remark, create_time, create_uid, update_time, update_uid, del_time, del_uid, is_del) FROM stdin;
2	设备通行记录获取任务                                                  	Device_Pass_Records                     	1	"* * * * *"                                                 	{"deviceId":"xyz","passPic":"http://"}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  	lalala	2026-08-14 12:13:21.806712	1	2026-08-14 18:17:42.851196	1	\N	\N	f
4	获取设备状态任务                                                    	Get_Device_Status                       	1	"* * * * *"                                                 	{\n      "deviceId":"a896hy43m",\n      "status":1\n}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      	定时获取设备状态任务	2026-08-14 18:24:47.535613	1	2026-08-16 13:13:33.246262	1	\N	\N	f
5	每日考勤计算汇总                                                    	Work_Calculate                          	0	"* * * * *"                                                 	{\n     "uid":"666",\n     "location":"where",\n     "time": "13:30"\n}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     	考勤汇总	2026-08-16 13:30:08.36089	1	\N	\N	\N	\N	f
3	测试任务                                                        	Test_Tsk                                	0	“* * * * *”                                                 	{"testId":"123"}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        	喜欢可以去试一下	2026-08-14 18:20:52.879597	1	2026-08-14 18:20:59.852141	1	2026-08-14 18:21:05.938588	1	t
\.


--
-- TOC entry 5013 (class 0 OID 57384)
-- Dependencies: 238
-- Data for Name: sys_tsk_record; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sys_tsk_record (id, tsk_id, tsk_param, tsk_msg, status, start_time, end_time, create_time) FROM stdin;
1	2	{"deviceId":"123"}		1	2026-08-15 12:13:14	2026-08-15 12:13:15	2026-08-15 12:13:17
2	4	{"devId":"321"}		1	2026-08-15 12:13:17	2026-08-15 12:13:18	2026-08-15 12:13:19
\.


--
-- TOC entry 5004 (class 0 OID 41045)
-- Dependencies: 229
-- Data for Name: sys_user; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_user (id, account, password, realname, email, avatar, phone, status, is_del, create_time, create_uid, update_time, update_uid, del_time, del_uid, sex) FROM stdin;
1	xxiao	845d5f1153c27beed29f479640445148	潇潇	666@888.999	http://689.com/666.png	13713688888	1	f	2026-01-09 16:16:16	1	\N	\N	\N	\N	\N
3427597427773509	test001	9d1182f9eafa6672a186a03173cb585d	测试	\N	\N	15131136537	0	t	2026-07-08 16:49:37.265671	1	2026-07-08 16:55:04.300979	1	2026-07-10 12:50:45.523125	1	1
3428246730625093	007	9d1182f9eafa6672a186a03173cb585d	国产凌凌漆	\N	\N	\N	0	t	2026-07-10 12:51:38.470693	1	\N	\N	2026-07-10 12:54:19.324544	1	1
2	xyz	845d5f1153c27beed29f479640445148	字母哥	698@666.698	http://666.com/999.png	13713699999	1	f	2026-01-09 16:16:16	1	2026-08-03 18:12:36.573216	1	\N	\N	1
3436819583262789	test	9d1182f9eafa6672a186a03173cb585d	测试账号	\N	\N	13738390001	0	f	2026-08-03 18:14:40.070309	1	\N	\N	\N	\N	1
3428247620800581	007	9d1182f9eafa6672a186a03173cb585d	凌凌漆	\N	\N	15131136537	0	f	2026-07-10 12:55:15.798409	1	2026-08-04 16:42:29.685797	1	\N	\N	1
3437501901926469	tst007	9d1182f9eafa6672a186a03173cb585d	零零7	\N	\N	15131136537	0	f	2026-08-05 16:31:01.775638	1	\N	\N	\N	\N	1
\.


--
-- TOC entry 5005 (class 0 OID 41056)
-- Dependencies: 230
-- Data for Name: sys_user_menu; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_user_menu (user_id, menu_id) FROM stdin;
\.


--
-- TOC entry 5006 (class 0 OID 41061)
-- Dependencies: 231
-- Data for Name: sys_user_org; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_user_org (user_id, org_id) FROM stdin;
\.


--
-- TOC entry 5007 (class 0 OID 41066)
-- Dependencies: 232
-- Data for Name: sys_user_role; Type: TABLE DATA; Schema: public; Owner: xiaoxiao
--

COPY public.sys_user_role (user_id, role_id, id) FROM stdin;
1	1	1
2	3	3436819077431365
2	6	3436819077431366
3436819583262789	6	3436819583283269
3428247620800581	2	3437150825357381
3428247620800581	3	3437150825357382
3428247620800581	6	3437150825357383
3437501901926469	3	3437501902143557
3437501901926469	6	3437501902139461
\.


--
-- TOC entry 5036 (class 0 OID 0)
-- Dependencies: 235
-- Name: sys_config_id_seq; Type: SEQUENCE SET; Schema: public; Owner: xiaoxiao
--

SELECT pg_catalog.setval('public.sys_config_id_seq', 15, true);


--
-- TOC entry 5037 (class 0 OID 0)
-- Dependencies: 224
-- Name: sys_log_id_seq; Type: SEQUENCE SET; Schema: public; Owner: xiaoxiao
--

SELECT pg_catalog.setval('public.sys_log_id_seq', 1, false);


--
-- TOC entry 5038 (class 0 OID 0)
-- Dependencies: 233
-- Name: sys_role_menu_id_seq; Type: SEQUENCE SET; Schema: public; Owner: xiaoxiao
--

SELECT pg_catalog.setval('public.sys_role_menu_id_seq', 9, true);


--
-- TOC entry 5039 (class 0 OID 0)
-- Dependencies: 234
-- Name: sys_role_menu_id_seq1; Type: SEQUENCE SET; Schema: public; Owner: xiaoxiao
--

SELECT pg_catalog.setval('public.sys_role_menu_id_seq1', 37, true);


--
-- TOC entry 5040 (class 0 OID 0)
-- Dependencies: 236
-- Name: sys_tsk_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sys_tsk_id_seq', 5, true);


--
-- TOC entry 4818 (class 2606 OID 41072)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4824 (class 2606 OID 41074)
-- Name: sys_config PK_sys_config; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_config
    ADD CONSTRAINT "PK_sys_config" PRIMARY KEY (id);


--
-- TOC entry 4826 (class 2606 OID 41076)
-- Name: sys_log PK_sys_log; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_log
    ADD CONSTRAINT "PK_sys_log" PRIMARY KEY (id);


--
-- TOC entry 4828 (class 2606 OID 41078)
-- Name: sys_menu PK_sys_menu; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_menu
    ADD CONSTRAINT "PK_sys_menu" PRIMARY KEY (id);


--
-- TOC entry 4830 (class 2606 OID 41080)
-- Name: sys_org PK_sys_org; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_org
    ADD CONSTRAINT "PK_sys_org" PRIMARY KEY (id);


--
-- TOC entry 4832 (class 2606 OID 41082)
-- Name: sys_role PK_sys_role; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_role
    ADD CONSTRAINT "PK_sys_role" PRIMARY KEY (id);


--
-- TOC entry 4834 (class 2606 OID 41084)
-- Name: sys_role_menu PK_sys_role_menu; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_role_menu
    ADD CONSTRAINT "PK_sys_role_menu" PRIMARY KEY (role_id, menu_id);


--
-- TOC entry 4836 (class 2606 OID 41086)
-- Name: sys_user PK_sys_user; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_user
    ADD CONSTRAINT "PK_sys_user" PRIMARY KEY (id);


--
-- TOC entry 4838 (class 2606 OID 41088)
-- Name: sys_user_menu PK_sys_user_menu; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_user_menu
    ADD CONSTRAINT "PK_sys_user_menu" PRIMARY KEY (user_id, menu_id);


--
-- TOC entry 4840 (class 2606 OID 41090)
-- Name: sys_user_org PK_sys_user_org; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_user_org
    ADD CONSTRAINT "PK_sys_user_org" PRIMARY KEY (org_id, user_id);


--
-- TOC entry 4842 (class 2606 OID 41092)
-- Name: sys_user_role PK_sys_user_role; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.sys_user_role
    ADD CONSTRAINT "PK_sys_user_role" PRIMARY KEY (role_id, user_id);


--
-- TOC entry 4822 (class 2606 OID 41094)
-- Name: prtcl_grpc prtcl_grpc_pkey; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.prtcl_grpc
    ADD CONSTRAINT prtcl_grpc_pkey PRIMARY KEY (id);


--
-- TOC entry 4820 (class 2606 OID 41096)
-- Name: prtcl prtcl_pkey; Type: CONSTRAINT; Schema: public; Owner: xiaoxiao
--

ALTER TABLE ONLY public.prtcl
    ADD CONSTRAINT prtcl_pkey PRIMARY KEY (id);


--
-- TOC entry 4844 (class 2606 OID 57383)
-- Name: sys_tsk sys_tsk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_tsk
    ADD CONSTRAINT sys_tsk_pkey PRIMARY KEY (id);


--
-- TOC entry 4846 (class 2606 OID 57392)
-- Name: sys_tsk_record sys_tsk_record_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_tsk_record
    ADD CONSTRAINT sys_tsk_record_pkey PRIMARY KEY (id);


-- Completed on 2026-08-16 22:20:05

--
-- PostgreSQL database dump complete
--

\unrestrict oe0ZtKDoRbUJ1dlF6lWmfZdUmRNHwh29IrfAobYVpUSubg73UtDRArgi79rMy1a

